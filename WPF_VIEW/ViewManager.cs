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
        /// <summary>
        /// Словарь для хранения соответствий между ViewModel и их окнами.
        /// </summary>
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

        /// <summary>
        /// Обрабатывает событие готовности новой ViewModel к отображению.
        /// Создает соответствующее окно, устанавливает DataContext и отображает его.
        /// </summary>
        /// <param name="vm">ViewModel, готовая к отображению.</param>
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

        /// <summary>
        /// Обрабатывает событие закрытия текущей ViewModel.
        /// Скрывает все окна, кроме главного.
        /// </summary>
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

        /// <summary>
        /// Создает окно для указанной ViewModel.
        /// </summary>
        /// <param name="vm">ViewModel, для которой нужно создать окно.</param>
        /// <returns>Созданное окно или null, если ViewModel не поддерживается.</returns>
        private Window CreateWindowForViewModel(ViewModelBase vm)
        {
            if (vm == null) return null;

            if (vm is ViewModelMain viewModelMain)
                return new MainWindow(viewModelMain);
            else if (vm is UpdateReaderViewModel viewModelUpdateReader)
                return new UpdateReader(viewModelUpdateReader);
            else if (vm is AddReaderViewModel viewModelAddReader)
                return new AddReader(viewModelAddReader);
            else if (vm is BookViewModel viewModelBookViewModel)
                return new AddBook(viewModelBookViewModel);
            else if (vm is ReturnGiveBookViewModel viewModelReturnGiveBook)
                return new ReturnGiveBook(viewModelReturnGiveBook);
            else
                return null;
        }

        /// <summary>
        /// Корректно завершает работу менеджера окон.
        /// Отписывается от событий, закрывает все окна и очищает ресурсы.
        /// </summary>
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
