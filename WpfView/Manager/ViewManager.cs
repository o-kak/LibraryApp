using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Presenter.ViewModel;
using WpfView;

namespace WpfView.Manager
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

                if (vm is ViewModelMAin)
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

        private Window CreateWindowForViewModel(ViewModelBase vm)
        {
            if (vm == null) return null;

            if (vm is ViewModelMAin)
                return new MainWindow();
            else if (vm is ReaderViewModel)
                return new AddReader();
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
                view.Close();
                _windows.Remove(vm);
            }
        }

    }
}
