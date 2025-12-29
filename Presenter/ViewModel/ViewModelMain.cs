using BusinessLogic;
using Model;
using Ninject;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    public class ViewModelMain : ViewModelBase
    {
        private readonly IBookService _bookService;
        private readonly IReaderService _readerService;
        private readonly ILoan _loanService;
        private readonly VMManager _vmManager;

        private BindingList<ReaderEventArgs> _readers;
        private BindingList<BookEventArgs> _books;
        private ReaderEventArgs _selectedReader;
        private BindingList<BookEventArgs> _selectedReaderBooks; 
        private BookEventArgs _selectedBook;

        private string _genreFilter;
        private string _authorFilter;
        private string _statusMessage;

        public BindingList<ReaderEventArgs> Readers
        {
            get => _readers;
            set
            {
                if (_readers == value)
                    return;
                _readers = value;
                OnPropertyChanged();
            }
        }

        public BindingList<BookEventArgs> Books
        {
            get => _books;
            set
            {
                if (_books == value)
                    return;
                _books = value;
                OnPropertyChanged();
            }
        }

        public BindingList<BookEventArgs> SelectedReaderBooks 
        {
            get => _selectedReaderBooks;
            set 
            {
                if( _selectedReaderBooks == value)
                    return;
                _selectedReaderBooks = value;
                OnPropertyChanged();
            }
        }

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader == value)
                    return;
                _selectedReader = value;
                OnPropertyChanged();
                if (UpdateReaderCommand is RelayCommand updateCmd)
                    updateCmd.RaiseCanExecuteChanged();
                if (DeleteReaderCommand is RelayCommand deleteCmd)
                    deleteCmd.RaiseCanExecuteChanged();

            }
        }

        public BookEventArgs SelectedBook
        {
            get => _selectedBook;
            set
            {
                if (_selectedBook == value)
                    return;
                _selectedBook = value;
                OnPropertyChanged();
                if (DeleteBookCommand is RelayCommand deletebookCmd)
                    deletebookCmd.RaiseCanExecuteChanged();
            }
        }

        public string GenreFilter
        {
            get => _genreFilter;
            set
            {
                if (_genreFilter == value)
                    return;
                _genreFilter = value;
                OnPropertyChanged();
            }
        }

        public string AuthorFilter
        {
            get => _authorFilter;
            set
            {
                if (_authorFilter == value)
                    return;
                _authorFilter = value;
                OnPropertyChanged();
            }
        }

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

        public bool CanDoSomethingReader => SelectedReader != null;
        public bool CanDeleteBook => SelectedBook != null;

        public ICommand AddReaderCommand { get; }
        public ICommand UpdateReaderCommand { get; }
        public ICommand DeleteReaderCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand RefreshBooksCommand { get; }
        public ICommand FilterByGenreCommand { get; }
        public ICommand FilterByAuthorCommand { get; }
        public ICommand GiveReturnBookCommand { get; }
        public ICommand LoadReadersCommand { get; }
        public ICommand LoadBooksCommand { get; }
        public ICommand ExitCommand { get; }

        public ViewModelMain(VMManager vmManager)
        {

            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));

            _bookService = new StandardKernel(new SimpleConfigModule()).Get<BookService>(); 
            _readerService = new StandardKernel(new SimpleConfigModule()).Get<ReaderService>(); 
            _loanService = new StandardKernel(new SimpleConfigModule()).Get<LoanService>(); 

            Readers = new BindingList<ReaderEventArgs>();
            Books = new BindingList<BookEventArgs>();


            AddReaderCommand = new RelayCommand(AddReader);
            UpdateReaderCommand = new RelayCommand(UpdateReader, () => CanDoSomethingReader);
            DeleteReaderCommand = new RelayCommand(DeleteReader, () => CanDoSomethingReader);
            AddBookCommand = new RelayCommand(AddBook);
            DeleteBookCommand = new RelayCommand(DeleteBook, () => CanDeleteBook);
            RefreshBooksCommand = new RelayCommand(LoadBooks);
            FilterByGenreCommand = new RelayCommand(FilterByGenre);
            FilterByAuthorCommand = new RelayCommand(FilterByAuthor);
            GiveReturnBookCommand = new RelayCommand(GiveReturnBook,() => CanDoSomethingReader);
            LoadReadersCommand = new RelayCommand(LoadReaders);
            LoadBooksCommand = new RelayCommand(LoadBooks);
            ExitCommand = new RelayCommand(ExitApp);

            _bookService.DataChanged += OnBooksDataChanged;
            _readerService.DataChanged += OnReadersDataChanged;

            LoadReaders();
            LoadBooks();
        }

        private void LoadReaders()
        {
            _readerService.InvokeDataChanged();
            StatusMessage = "Обновление списка читателей...";
        }

        private void LoadBooks()
        {
            _bookService.InvokeDataChanged();
            StatusMessage = "Обновление списка книг...";
        }

        private void OnBooksDataChanged(IEnumerable<Book> books)
        {
            UpdateBooksCollection(books);
        }

        private void OnReadersDataChanged(IEnumerable<Reader> readers)
        {
            UpdateReadersCollection(readers);
        }

        private void UpdateBooksCollection(IEnumerable<Book> modelBooks)
        {
            Books.Clear();
            foreach (var book in modelBooks)
            {
                var bookDTO = new BookEventArgs
                {
                    Id = book.Id,
                    Title = book.Title,
                    Genre = book.Genre,
                    Author = book.Author,
                    IsAvailable = book.IsAvailable,
                    ReaderId = book.ReaderId
                };
                Books.Add(bookDTO);
            }
        }

        private void UpdateReadersCollection(IEnumerable<Reader> modelReaders)
        {
            Readers.Clear();
            foreach (var reader in modelReaders)
            {
                var readerDTO = new ReaderEventArgs
                {
                    Id = reader.Id,
                    Name = reader.Name,
                    Address = reader.Address
                };
                Readers.Add(readerDTO);
            }
        }

        private void GiveReturnBook()
        {
            var giveReturnBookVM = _vmManager.CreateReturnGiveBookViewModel();
        }
        private void AddReader()
        {
            var readerVM = _vmManager.CreateAddReaderViewModel();
        }

        private void UpdateReader()
        {
            if (SelectedReader != null)
            {
                var readerVM = _vmManager.CreateUpdateReaderViewModel(SelectedReader);
            }
        }

        private void DeleteReader()
        {
            if (SelectedReader == null)
            {
                StatusMessage = "Не выбран читатель для удаления";
                return;
            }

            try
            {
                var readerToDelete = SelectedReader;

                var borrowedBooks = _loanService.GetReadersBorrowedBooks(readerToDelete.Id);

                if (borrowedBooks != null && borrowedBooks.Any())
                {
                    foreach (var book in borrowedBooks)
                    {
                        if (book.ReaderId == readerToDelete.Id)
                        {
                            _loanService.ReturnBook(book.Id, readerToDelete.Id);
                        }
                    }

                    StatusMessage = $"Возвращено {borrowedBooks.Count()} книг читателя {readerToDelete.Name}";

                    Task.Delay(1000).Wait();
                }

                _readerService.Delete(readerToDelete.Id);
                LoadReaders();
                LoadBooks();
                StatusMessage = $"Читатель {readerToDelete.Name} удален";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка удаления: {ex.Message}";
            }
        }

        private void AddBook()
        {
            var bookVM = _vmManager.CreateBookViewModel();
        }

        private void DeleteBook()
        {
            if (SelectedBook != null)
            {
                _bookService.Delete(SelectedBook.Id);
                LoadBooks();
                StatusMessage = $"Книга |{SelectedBook.Title}| удалена";
            }
        }

        private void FilterByGenre()
        {
            if (string.IsNullOrWhiteSpace(GenreFilter))
            {
                LoadBooks();
                return;
            }
            var filteredBooks = _bookService.FilterByGenre(GenreFilter);
            UpdateBooksCollection(filteredBooks);
            StatusMessage = $"Отфильтровано по жанру: {GenreFilter}";
        }

        private void FilterByAuthor()
        {
            if (string.IsNullOrWhiteSpace(AuthorFilter))
            {
                LoadBooks();
                return;
            }

            var filteredBooks = _bookService.FilterByAuthor(AuthorFilter);
            UpdateBooksCollection(filteredBooks);
            StatusMessage = $"Отфильтровано по автору: {AuthorFilter}";
        }

        private void ExitApp()
        {
            System.Environment.Exit(0);
        }

        public override void Dispose()
        {
            _bookService.DataChanged -= OnBooksDataChanged;
            _readerService.DataChanged -= OnReadersDataChanged;
            base.Dispose();
        }
    }
}
