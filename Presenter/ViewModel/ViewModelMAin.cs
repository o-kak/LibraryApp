using BusinessLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows; 

namespace Presenter.ViewModel
{
    public class ViewModelMAin : ViewModelBase
    {
        private readonly IBookService _bookService;
        private readonly IReaderService _readerService;
        private readonly ILoan _loanService;
        private readonly VMManager _vmManager;

        private string _statusMessage;
        public string StatusMessage 
        {
            get => _statusMessage;
            set 
            {
                if (_statusMessage == value)
                    return;
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowBooksCommand { get; }
        public ICommand ShowReadersCommand { get; }
        public ICommand ShowLoanCommand { get; }
        public ICommand ExitCommand { get; }

        public ViewModelMAin(VMManager vmManager)
        {

            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));


            ShowBooksCommand = new RelayCommand(() => ShowBooks());
            ShowReadersCommand = new RelayCommand(() => ShowReaders());
            ShowLoanCommand = new RelayCommand(() => ShowLoans());
            ExitCommand = new RelayCommand(ExitApp);
        }
        public VMManager Manager => _vmManager;

        private void ShowBooks()
        {
            var bookViewModel = _vmManager.CreateBookViewModel();
            _vmManager.CurrentViewModel = bookViewModel;
        }

        private void ShowReaders() 
        {
            var readerViewModel = _vmManager.CreateReaderViewModel();
            _vmManager.CurrentViewModel = readerViewModel;
        }
        private void ShowLoans()
        {
            var loanViewModel = _vmManager.CreateLoanViewModel();
            _vmManager.CurrentViewModel = loanViewModel;
        }
        private void ExitApp()
        {
            System.Environment.Exit(0);
        }

    }
}
