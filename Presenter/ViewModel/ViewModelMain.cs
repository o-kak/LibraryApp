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

        /// <summary>
        /// Список читателей для отображения в DataGrid
        /// </summary>
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

        /// <summary>
        /// Список книг для отображения в DataGrid
        /// </summary>
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

        /// <summary>
        /// Список книг читателей
        /// </summary>
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

        /// <summary>
        /// Выбранный читатель из списка читателей
        /// </summary>
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

        /// <summary>
        /// Выбранная книга из списка книг
        /// </summary>
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

        /// <summary>
        /// Значение фильтра по жанру книг
        /// </summary>
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

        /// <summary>
        /// Значение фильтра по автору книг
        /// </summary>
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

        /// <summary>
        /// Сообщение о статусе операций для отображения в статусной строке
        /// </summary>
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

        /// <summary>
        /// Команда для добавления нового читателя
        /// </summary>
        public ICommand AddReaderCommand { get; }

        /// <summary>
        /// Команда для редактирования выбранного читателя
        /// </summary>
        public ICommand UpdateReaderCommand { get; }

        /// <summary>
        /// Команда для удаления выбранного читателя
        /// </summary>
        public ICommand DeleteReaderCommand { get; }

        /// <summary>
        /// Команда для добавления новой книги
        /// </summary>
        public ICommand AddBookCommand { get; }

        /// <summary>
        /// Команда для удаления выбранной книги
        /// </summary>
        public ICommand DeleteBookCommand { get; }


        /// <summary>
        /// Команда для фильтрации книг по жанру
        /// </summary>
        public ICommand FilterByGenreCommand { get; }

        /// <summary>
        /// Команда для фильтрации книг по автору
        /// </summary>
        public ICommand FilterByAuthorCommand { get; }

        /// <summary>
        /// Команда для выдачи или возврата книги
        /// </summary>
        public ICommand GiveReturnBookCommand { get; }

        /// <summary>
        /// Команда для обновления списка читателей
        /// </summary>
        public ICommand LoadReadersCommand { get; }

        /// <summary>
        /// Команда для обновления списка книг
        /// </summary>
        public ICommand LoadBooksCommand { get; }

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
            FilterByGenreCommand = new RelayCommand(FilterByGenre);
            FilterByAuthorCommand = new RelayCommand(FilterByAuthor);
            GiveReturnBookCommand = new RelayCommand(GiveReturnBook,() => CanDoSomethingReader);
            LoadReadersCommand = new RelayCommand(LoadReaders);
            LoadBooksCommand = new RelayCommand(LoadBooks);

            _bookService.DataChanged += OnBooksDataChanged;
            _readerService.DataChanged += OnReadersDataChanged;

            LoadReaders();
            LoadBooks();
        }

        /// <summary>
        /// Обновляет список читателей
        /// </summary>
        private void LoadReaders()
        {
            _readerService.InvokeDataChanged();
            StatusMessage = "Обновление списка читателей...";
        }

        /// <summary>
        /// Обновляет список книг
        /// </summary>
        private void LoadBooks()
        {
            _bookService.InvokeDataChanged();
            StatusMessage = "Обновление списка книг...";
        }

        /// <summary>
        /// Обрабатывает событие изменения данных книг
        /// </summary>
        /// <param name="books">Обновленная коллекция книг из базы данных</param>
        private void OnBooksDataChanged(IEnumerable<Book> books)
        {
            UpdateBooksCollection(books);
        }

        /// <summary>
        /// Обрабатывает событие изменения данных читателей
        /// </summary>
        /// <param name="readers">Обновленная коллекция читателей из базы данных</param>
        private void OnReadersDataChanged(IEnumerable<Reader> readers)
        {
            UpdateReadersCollection(readers);
        }

        /// <summary>
        /// Обновляет коллекцию книг для отображения в UI
        /// </summary>
        /// <param name="modelBooks">Коллекция книг из модели данных</param>
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

        /// <summary>
        /// Обновляет коллекцию читателей для отображения в UI
        /// </summary>
        /// <param name="modelReaders">Коллекция читателей из модели данных</param>
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

        /// <summary>
        /// Открывает окно для выдачи или возврата книги выбранному читателю
        /// </summary>
        private void GiveReturnBook()
        {
            var giveReturnBookVM = _vmManager.CreateReturnGiveBookViewModel();
        }

        /// <summary>
        /// Открывает окно для добавления нового читателя
        /// </summary>
        private void AddReader()
        {
            var readerVM = _vmManager.CreateAddReaderViewModel();
        }

        /// <summary>
        /// Открывает окно для редактирования выбранного читателя
        /// </summary>
        private void UpdateReader()
        {
            if (SelectedReader != null)
            {
                var readerVM = _vmManager.CreateUpdateReaderViewModel(SelectedReader);
            }
        }

        /// <summary>
        /// Удаляет выбранного читателя из базы данных.
        /// Перед удалением возвращает все книги, взятые читателем.
        /// </summary>
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

        /// <summary>
        /// Открывает окно для добавления новой книги
        /// </summary>
        private void AddBook()
        {
            var bookVM = _vmManager.CreateBookViewModel();
        }

        /// <summary>
        /// Удаляет выбранную книгу из базы данных
        /// </summary>
        private void DeleteBook()
        {
            if (SelectedBook == null)
            {
                StatusMessage = "Не выбрана книга для удаления";
                return;
            }
            try
            {
                var bookToDelete = SelectedBook;
                _bookService.Delete(bookToDelete.Id);
                LoadBooks();
                StatusMessage = $"Книга |{bookToDelete.Title}| удалена";
            }
            catch (Exception ex) 
            {
                StatusMessage = $"Ошибка удаления: {ex.Message}";
            }
        }

        /// <summary>
        /// Фильтрует список книг по заданному жанру
        /// </summary>
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

        /// <summary>
        /// Фильтрует список книг по заданному автору
        /// </summary>
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

        /// <summary>
        /// Освобождает ресурсы и отписывается от событий
        /// </summary>
        public override void Dispose()
        {
            _bookService.DataChanged -= OnBooksDataChanged;
            _readerService.DataChanged -= OnReadersDataChanged;
            base.Dispose();
        }
    }
}
