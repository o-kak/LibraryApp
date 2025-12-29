using Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_VIEW;

namespace WPF_VIEW
{
    public class ViewManager
    {
        Dictionary<ViewModelBase, Window> _windows;
        private VMManager _vmManager;
        private Window _mainWindow;

        public ViewManager()
        {
            _windows = new Dictionary<ViewModelBase, Window>();

            _vmManager = new VMManager();
            _vmManager.VMMReadyEvent += OnViewModelReady;
            _vmManager.ViewModelClosedEvent += OnViewModelClosed;


            _vmManager.Start();
        }

        private void OnViewModelReady(ViewModelBase vm)
        {
            if (vm == null) return;

            var window = CreateWindowForViewModel(vm);
            if (window != null)
            {
                _windows[vm] = window;
                window.DataContext = vm;

                if (vm is ViewModelMain)
                {
                    _mainWindow = window;
                    window.Show();
                }
                else
                {
                    window.Owner = _mainWindow;
                    window.ShowDialog();
                } 
            }
        }
        private void OnViewModelClosed()
        {

            var windowsToClose = new List<ViewModelBase>();

            foreach (var kvp in _windows)
            {
                if (!(kvp.Key is ViewModelMain))
                {
                    windowsToClose.Add(kvp.Key);
                }
            }

            foreach (var vm in windowsToClose)
            {
                if (_windows.TryGetValue(vm, out var window))
                {
                    window.Hide();
                }
            }
        }

        private Window CreateWindowForViewModel(ViewModelBase vm)
        {
            if (vm == null) return null;

            if (vm is ViewModelMain viewModelMain)
                return new MainWindow(viewModelMain);
            else if (vm is ReaderViewModel)
                return new UpdateReader();
            else if (vm is AddReaderViewModel viewModelAddReader)
                return new AddReader(viewModelAddReader);
            else if (vm is BookViewModel)
                return new AddBook();
            else if (vm is ReturnGiveBookViewModel)
                return new ReturnGiveBook();
            else
                return null;
        }

        public void CloseView(ViewModelBase vm)
        {
            if (_windows.TryGetValue(vm, out Window view))
            {
                view.Hide();

            }
        }

        public void Shutdown()
        {
            _vmManager.VMMReadyEvent -= OnViewModelReady;
            _vmManager.ViewModelClosedEvent -= OnViewModelClosed;

            foreach (var window in _windows.Values.ToList())
            {
                window.Close();
            }
            _windows.Clear();
        }

    }
}
