using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Presenter.ViewModel;
using WpfView.Windows;

namespace WpfView.Manager
{
    public class ViewManager
    {
        Dictionary<ViewModelBase, View> _viewDictionary;
        private VMManager _vmManager;

        public ViewManager() 
        {
            _viewDictionary = new Dictionary<ViewModelBase, View>();

            var vmManager = new VMManager();
            vmManager.VMMReadyEvent += OnViewModelReady;
            vmManager.Start();
        }
        private void OnViewModelReady(ViewModelBase vm) 
        {
            if (vm == null) return;
            if (!_viewDictionary.ContainsKey(vm)) 
            {
                _viewDictionary.Add(vm, Fabric(vm));
            }

            _viewDictionary.TryGetValue(vm, out View view);
            view.Show();
        }

        private View Fabric(ViewModelBase vm) 
        {
            switch (vm) 
            {
                case ViewModelMAin mainVM:
                    var mainWindow = new MainWindow();
                    mainWindow.DataContext = mainVM;
                    return mainWindow;

                case BookViewModel bookVM:
                    var bookWindow = new AddBook();
                    bookWindow.DataContext = bookVM;
                    return bookWindow;

                case ReaderViewModel readerVM:
                    var readerWindow = new AddReader();
                    readerWindow.DataContext = readerVM;
                    return readerWindow;

                case LoanViewModel loanVM:
                    var returnGiveBook = new ReturnGiveBook();
                    returnGiveBook.DataContext = loanVM;
                    return returnGiveBook;


                default:
                    return new View(vm);

            }
        }

        public void CloseView(ViewModelBase vm)
        {
            if (_viewDictionary.TryGetValue(vm, out View view))
            {
                view.Close();
                _viewDictionary.Remove(vm);
            }
        }

    }
}
