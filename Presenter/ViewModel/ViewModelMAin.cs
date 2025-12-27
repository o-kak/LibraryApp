using BusinessLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using Shared;

namespace Presenter.ViewModel
{
    public class ViewModelMAin : ViewModelBase
    {
        private readonly IBookService _bookService;
        private readonly IReaderService _readerService;
        private readonly ILoan _loanService;
        private readonly VMManager _vmManager;

        private BindingList<ReaderEventArgs> _readers;
        private BindingList<BookEventArgs> _books;
        private ReaderEventArgs _selectedReader;
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

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader == value)
                    return;
                _selectedReader = value;
                OnPropertyChanged();
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

        public ViewModelMAin(VMManager vmManager)
        {

            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));

            _bookService = ServiceLocator.BookService;
            _readerService = ServiceLocator.ReaderService;
            _loanService = ServiceLocator.LoanService;

            Readers = new BindingList<ReaderEventArgs>();
            Books = new BindingList<BookEventArgs>();


            AddReaderCommand = new RelayCommand(AddReader);
            UpdateReaderCommand = new RelayCommand(UpdateReader, () => SelectedReader != null);
            DeleteReaderCommand = new RelayCommand(DeleteReader, () => SelectedReader != null);
            AddBookCommand = new RelayCommand(AddBook);
            DeleteBookCommand = new RelayCommand(DeleteBook, () => SelectedBook != null);
            RefreshBooksCommand = new RelayCommand(LoadBooks);
            FilterByGenreCommand = new RelayCommand(FilterByGenre);
            FilterByAuthorCommand = new RelayCommand(FilterByAuthor);
            GiveReturnBookCommand = new RelayCommand(GiveReturnBook);
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
            var readerVM = _vmManager.CreateReaderViewModel(null);
        }

        private void UpdateReader() 
        {
            if (SelectedReader != null) 
            {
                var readerVM = _vmManager.CreateReaderViewModel(SelectedReader);
            }
        }

        private void DeleteReader() 
        {
            if (SelectedReader != null) 
            {
                _readerService.Delete(SelectedReader.Id);
                LoadReaders();
                StatusMessage = $"Читатель {SelectedReader.Name} удален";
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
