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
                OnPropertyChanged();
            }
        }

        public BindingList<ReaderEventArgs> Readers
        {
            get => _readers;
            set
            {
                _readers = value;
                OnPropertyChanged();
            }
        }

        public BindingList<BookEventArgs> BorrowedBooks
        {
            get => _borrowedBooks;
            set
            {
                _borrowedBooks = value;
                OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanGiveBook));
                    LoadReadersBorrowedBooks();
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
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanReturnBook));
                }
            }
        }

        public bool CanGiveBook => SelectedBookToLoan != null && SelectedReader != null;
        public bool CanReturnBook => SelectedBookToReturn != null;

        public ICommand GiveBookCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public LoanViewModel(ILoan loanService, IBookService bookService, IReaderService readerService)
        {
            _loanService = loanService;
            _bookService = bookService;
            _readerService = readerService;

            AvailableBooks = new BindingList<BookEventArgs>();
            Readers = new BindingList<ReaderEventArgs>();
            BorrowedBooks = new BindingList<BookEventArgs>();

            // Подписываемся на события изменения данных
            _bookService.DataChanged += OnBookDataChanged;
            _readerService.DataChanged += OnReaderDataChanged;

            GiveBookCommand = new RelayCommand(GiveBook, () => CanGiveBook);
            ReturnBookCommand = new RelayCommand(ReturnBook, () => CanReturnBook);
            LoadDataCommand = new RelayCommand(LoadData);
            ClearSelectionCommand = new RelayCommand(ClearSelection);

            LoadData();
        }

        private void OnBookDataChanged(IEnumerable<Book> books)
        {
            LoadAvailableBooks();
            LoadBorrowedBooks();
        }

        private void OnReaderDataChanged(IEnumerable<Reader> readers)
        {
            LoadReaders();
        }

        private BookEventArgs ConvertToBookEventArgs(Book book)
        {
            return new BookEventArgs
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                IsAvailable = book.IsAvailable,
                ReaderId = book.ReaderId
            };
        }

        private ReaderEventArgs ConvertToReaderEventArgs(Reader reader)
        {
            return new ReaderEventArgs
            {
                Id = reader.Id,
                Name = reader.Name,
                Address = reader.Address
            };
        }

        private void LoadData()
        {
            LoadAvailableBooks();
            LoadReaders();
            LoadBorrowedBooks();
        }

        private void LoadAvailableBooks()
        {
            var books = _bookService.GetAvailableBooks();
            AvailableBooks.Clear();

            foreach (var book in books)
            {
                AvailableBooks.Add(ConvertToBookEventArgs(book));
            }
        }

        private void LoadReaders()
        {
            var readers = _readerService.GetAllReaders();
            Readers.Clear();

            foreach (var reader in readers)
            {
                Readers.Add(ConvertToReaderEventArgs(reader));
            }
        }

        private void LoadBorrowedBooks()
        {
            var books = _bookService.GetBorrowedBooks();
            BorrowedBooks.Clear();

            foreach (var book in books)
            {
                BorrowedBooks.Add(ConvertToBookEventArgs(book));
            }
        }

        private void LoadReadersBorrowedBooks()
        {
            if (SelectedReader != null)
            {
                var borrowedBooks = _loanService.GetReadersBorrowedBooks(SelectedReader.Id);
                BorrowedBooks.Clear();

                foreach (var book in borrowedBooks)
                {
                    BorrowedBooks.Add(ConvertToBookEventArgs(book));
                }
            }
        }

        private void GiveBook()
        {
            if (!CanGiveBook)
            {
                return;
            }

            try
            {
                _loanService.GiveBook(SelectedBookToLoan.Id, SelectedReader.Id);

                // Данные обновятся автоматически через события DataChanged
                ClearSelection();
            }
            catch (InvalidOperationException ex)
            {
                // Здесь можно показать сообщение об ошибке
                System.Windows.MessageBox.Show(ex.Message, "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void ReturnBook()
        {
            if (!CanReturnBook || SelectedBookToReturn.ReaderId == null)
            {
                return;
            }

            try
            {
                _loanService.ReturnBook(SelectedBookToReturn.Id, SelectedBookToReturn.ReaderId.Value);

                // Данные обновятся автоматически через события DataChanged
                SelectedBookToReturn = null;
            }
            catch (InvalidOperationException ex)
            {
                // Здесь можно показать сообщение об ошибке
                System.Windows.MessageBox.Show(ex.Message, "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
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
