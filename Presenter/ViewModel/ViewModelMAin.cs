using BusinessLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ViewModelMAin(IBookService bookService, IReaderService readerService, ILoan loanService, VMManager vmManager = null)
        {
            _bookService = bookService;
            _readerService = readerService;
            _loanService = loanService;
            _vmManager = vmManager ?? new VMManager();


            ShowBooksCommand = new RelayCommand(() => ShowBooks());
            ShowReadersCommand = new RelayCommand(() => ShowReaders());
            ShowLoanCommand = new RelayCommand(() => ShowLoans());
           
        }
        public VMManager Manager => _vmManager;

        private void ShowBooks()
        {
            var bookViewModel = new BookViewModel(_bookService);
            _vmManager.CurrentViewModel = bookViewModel;
        }

        private void ShowReaders() 
        {
            var readerViewModel = new ReaderViewModel(_readerService, _loanService);
            _vmManager.CurrentViewModel = readerViewModel;
        }
        private void ShowLoans()
        {
            var loanViewModel = new LoanViewModel(_loanService, _bookService, _readerService);
            _vmManager.CurrentViewModel = loanViewModel;
        }

    }
}
