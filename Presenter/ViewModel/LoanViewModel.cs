using BusinessLogic;
using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    public class LoanViewModel : ViewModelBase
    {
        private readonly ILoan _loanService;
        private readonly IBookService _bookService;
        private readonly IReaderService _readerService;

        private BindingList<BookEventArgs> _availableBooks;
        private BindingList<ReaderEventArgs> _readers;
        private BindingList<BookEventArgs> _borrowedBooks;

        private BookEventArgs _selectedBookToLoan;
        private ReaderEventArgs _selectedReader;
        private BookEventArgs _selectedBookToReturn;

        public BindingList<BookEventArgs> AvailableBooks
        {
            get => _availableBooks;
            set
            {
                _availableBooks = value;
                OnPropertyChanged(nameof(AvailableBooks));
            }
        }

        public BindingList<ReaderEventArgs> Readers
        {
            get => _readers;
            set
            {
                _readers = value;
                OnPropertyChanged(nameof(Readers));
            }
        }

        public BindingList<BookEventArgs> BorrowedBooks
        {
            get => _borrowedBooks;
            set
            {
                _borrowedBooks = value;
                OnPropertyChanged(nameof(BorrowedBooks));
            }
        }

        public BookEventArgs SelectedBookToLoan
        {
            get => _selectedBookToLoan;
            set
            {
                if (_selectedBookToLoan != value)
                {
                    _selectedBookToLoan = value;
                    OnPropertyChanged(nameof(SelectedBookToLoan));
                    OnPropertyChanged(nameof(CanGiveBook));
                }
            }
        }

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader != value)
                {
                    _selectedReader = value;
                    OnPropertyChanged(nameof(SelectedReader));
                    OnPropertyChanged(nameof(CanGiveBook));

                    if (value != null)
                    {
                        LoadReadersBorrowedBooks(value.Id);
                    }
                }
            }
        }

        public BookEventArgs SelectedBookToReturn
        {
            get => _selectedBookToReturn;
            set
            {
                if (_selectedBookToReturn != value)
                {
                    _selectedBookToReturn = value;
                    OnPropertyChanged(nameof(SelectedBookToLoan));
                    OnPropertyChanged(nameof(CanReturnBook));
                }
            }
        }

        public bool CanGiveBook => SelectedBookToLoan != null && SelectedReader != null;
        public bool CanReturnBook => SelectedBookToReturn != null && _selectedBookToReturn.ReaderId.HasValue;

        public ICommand GiveBookCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand StartupCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public LoanViewModel(ILoan loanService, IBookService bookService, IReaderService readerService)
        {
            _loanService = loanService;
            _bookService = bookService;
            _readerService = readerService;

            AvailableBooks = new BindingList<BookEventArgs>();
            Readers = new BindingList<ReaderEventArgs>();
            BorrowedBooks = new BindingList<BookEventArgs>();

            _readerService.DataChanged += OnReaderDataChanged;

            GiveBookCommand = new RelayCommand(GiveBook, () => CanGiveBook);
            ReturnBookCommand = new RelayCommand(ReturnBook, () => CanReturnBook);
            StartupCommand = new RelayCommand(Startup);
            ClearSelectionCommand = new RelayCommand(ClearSelection);

            Startup();
        }

        private void OnBookDataChanged(IEnumerable<Book> books)
        {
            LoadAvailableBooks();
            LoadBorrowedBooks();
        }

        private void OnReaderDataChanged(IEnumerable<Reader> readers)
        {
            LoadReaders(readers.ToList());
        }

        

        private void Startup()
        {
            _readerService.InvokeDataChanged();
        }

        private void LoadReaders(List<Reader> readers)
        {
            var dtos = new List<ReaderEventArgs>();

            foreach (var reader in readers.OrderBy(r => r.Name))
            {
                dtos.Add(new ReaderEventArgs
                {
                    Id = reader.Id,
                    Name = reader.Name,
                    Address = reader.Address
                });
            }

            Readers = new BindingList<ReaderEventArgs>(dtos);
        }

        private void LoadAvailableBooks()
        {
            var availableBooks = _bookService.GetAvailableBooks().ToList();
            var dtos = new List<BookEventArgs>();

            foreach (var book in availableBooks.OrderBy(b => b.Title))
            {
                dtos.Add(new BookEventArgs
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    IsAvailable = book.IsAvailable
                });
            }

            AvailableBooks = new BindingList<BookEventArgs>(dtos);
        }

        private void LoadBorrowedBooks()
        {
            var borrowedBooks = _bookService.GetBorrowedBooks().ToList();
            var dtos = new List<BookEventArgs>();

            foreach (var book in borrowedBooks.OrderBy(b => b.Title))
            {
                dtos.Add(new BookEventArgs
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = book.ReaderId,
                    IsAvailable = book.IsAvailable
                });
            }
            BorrowedBooks = new BindingList<BookEventArgs>(dtos);
        }

        private void LoadReadersBorrowedBooks(int readerId)
        {
            var books = _loanService.GetReadersBorrowedBooks(readerId).ToList();
            var dtos = books.Select(b => new BookEventArgs
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                ReaderId = readerId
            }).ToList();

            BorrowedBooks = new BindingList<BookEventArgs>(dtos);
        }

        private void GiveBook()
        {
            if (!CanGiveBook)
            {
                return;
            }

            _loanService.GiveBook(_selectedBookToLoan.Id, SelectedReader.Id);
            ClearSelection();
        }

        private void ReturnBook()
        {
            if (!CanReturnBook || SelectedBookToReturn.ReaderId == null)
            {
                return;
            }

            _loanService.ReturnBook(_selectedBookToReturn.Id, _selectedBookToReturn.ReaderId.Value);
            _selectedBookToReturn = null;
        }

        private void ClearSelection()
        {
            SelectedBookToLoan = null;
            SelectedReader = null;
        }

        public void Dispose()
        {
            _bookService.DataChanged -= OnBookDataChanged;
            _readerService.DataChanged -= OnReaderDataChanged;
        }
    }
}
