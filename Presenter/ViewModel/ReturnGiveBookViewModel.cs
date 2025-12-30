using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Shared;
using Model;
using System.Windows.Input;
using Ninject;

namespace Presenter.ViewModel
{
    public class ReturnGiveBookViewModel : ViewModelBase
    {
        private readonly ILoan _loanService;
        private readonly IBookService _bookService;
        private readonly VMManager _vmManager;

        private ReaderEventArgs _selectedReader;
        private BookEventArgs _selectedReadersBook;
        private BookEventArgs _selectedAvailableBook;

        private BindingList<BookEventArgs> _readersBooks;
        private BindingList<BookEventArgs> _availableBooks;

        public ICommand GiveBookCommand { get; set; }
        public ICommand ReturnBookCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader != value)
                {
                    _selectedReader = value;
                    OnPropertyChanged();
                }
            }
        }

        public BookEventArgs SelectedReadersBook
        {
            get => _selectedReadersBook;
            set
            {
                if (_selectedReadersBook != value)
                {
                    _selectedReadersBook = value;
                    OnPropertyChanged();
                }
            }
        }
        public BookEventArgs SelectedAvailableBook
        {
            get => _selectedAvailableBook;
            set
            {
                if (_selectedAvailableBook != value)
                {
                    _selectedAvailableBook = value;
                    OnPropertyChanged();
                }
            }
        }

        public BindingList<BookEventArgs> ReadersBooks
        {
            get => _readersBooks;
            set
            {
                if (_readersBooks == value)
                    return;
                _readersBooks = value;
                OnPropertyChanged();
            }
        }

        public BindingList<BookEventArgs> AvailableBooks
        {
            get => _availableBooks;
            set
            {
                if (_availableBooks == value)
                    return;
                _availableBooks = value;
                OnPropertyChanged();
            }
        }

        public ReturnGiveBookViewModel(VMManager vmManager, ReaderEventArgs reader)
        {
            _vmManager = vmManager;
            SelectedReader = reader;

            _bookService = new StandardKernel(new SimpleConfigModule()).Get<BookService>();
            _loanService = new StandardKernel(new SimpleConfigModule()).Get<LoanService>();

            GiveBookCommand = new RelayCommand(GiveBook);
            ReturnBookCommand = new RelayCommand(ReturnBook);
            UpdateCommand = new RelayCommand(Update);

            ReadersBooks = new BindingList<BookEventArgs>();
            AvailableBooks = new BindingList<BookEventArgs>();

            LoadAvailableBooks();
            LoadReadersBooks();
        }

        private void LoadReadersBooks()
        {
            ReadersBooks.Clear();
            List<Book> books = _loanService.GetReadersBorrowedBooks(_selectedReader.Id).ToList();
            foreach (Book book in books)
            {
                BookEventArgs args = new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    IsAvailable = book.IsAvailable,
                    ReaderId = book.ReaderId,
                };
                ReadersBooks.Add(args);
            }
        }

        private void LoadAvailableBooks()
        {
            AvailableBooks.Clear();
            List<Book> books = _bookService.GetAvailableBooks().ToList();
            foreach (Book book in books)
            {
                BookEventArgs args = new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    IsAvailable = book.IsAvailable,
                    ReaderId = book.ReaderId,
                };
                AvailableBooks.Add(args);
            }
        }

        private void GiveBook()
        {
            _loanService.GiveBook(SelectedAvailableBook.Id, SelectedReader.Id);
            LoadAvailableBooks();
            LoadReadersBooks();
        }

        private void ReturnBook()
        {
            _loanService.ReturnBook(SelectedReadersBook.Id, SelectedReader.Id);
            LoadAvailableBooks();
            LoadReadersBooks();
        }

        private void Update()
        {
            LoadAvailableBooks();
            LoadReadersBooks();
            _vmManager.CloseCurrentView();
        }

        /// <summary>
        /// Освобождает ресурсы, используемые AddReaderViewModel.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
