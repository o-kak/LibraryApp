using BusinessLogic;
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

        private ViewModelBase _currentViewModel; 
        public ViewModelBase CurrentViewModel 
        {
            get => _currentViewModel;
            set 
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
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

        public ViewModelMAin(IBookService bookService, IReaderService readerService, ILoan loanService)
        {
            _bookService = bookService;
            _readerService = readerService;
            _loanService = loanService;


            ShowBooksCommand = new RelayCommand(() => ShowBooks());
            ShowReadersCommand = new RelayCommand(() => ShowReaders());
            ShowLoanCommand = new RelayCommand(() => ShowLoans());
           
        }

        private void ShowBooks() 
        {
            CurrentViewModel = new BookViewModel(_bookService);
        }

        private void ShowReaders() 
        {
            CurrentViewModel = new ReaderViewModel(_readerService);
        }
        private void ShowLoans()
        {
            CurrentViewModel = new LoanViewModel(_loanService, _bookService, _readerService);
        }

    }
}
