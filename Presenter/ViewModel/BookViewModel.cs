using BusinessLogic;
using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;

namespace Presenter.ViewModel
{
    public class BookViewModel : ViewModelBase
    {
        private readonly IBookService _bookService;
        private BindingList<BookEventArgs> _books;

        private BookEventArgs _selectedBook;

        private string _newBookTitle;
        private string _newBookAuthor;
        private string _newBookGenre;

        private bool _showAvailableOnly;
        private bool _showBorrowedOnly;

        private string _searchAuthor;
        private string _searchGenre;

        

        public BindingList<BookEventArgs> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        public string NewBookTitle
        {
            get => _newBookTitle;
            set { _newBookTitle = value; OnPropertyChanged();}
        }

        public string NewBookAuthor
        {
            get => _newBookAuthor;
            set { _newBookAuthor = value; OnPropertyChanged();}
        }

        public string NewBookGenre
        {
            get => _newBookGenre;
            set { _newBookGenre = value; OnPropertyChanged(); }
        }

        public BookEventArgs SelectedBook
        {
            get => _selectedBook;
            set { _selectedBook = value; OnPropertyChanged(); }
        }

        public string SearchAuthor
        {
            get => _searchAuthor;
            set { _searchAuthor = value; OnPropertyChanged(); }
        }

        public string SearchGenre
        {
            get => _searchGenre;
            set { _searchGenre = value; OnPropertyChanged(); }
        }


        public bool CanAddBook => !string.IsNullOrWhiteSpace(NewBookTitle) &&
                                 !string.IsNullOrWhiteSpace(NewBookAuthor) &&
                                 !string.IsNullOrWhiteSpace(NewBookGenre);
        public bool CanDeleteBook => SelectedBook != null;

        public ICommand StartupCommand { get; }
        public ICommand LoadBooksCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand AddBookCommand { get; }

        public ICommand GetBorrowedBooksCommand { get; }
        public ICommand GetAvailableBooksCommand { get; }


        public ICommand FilterByAuthorCommand { get; }
        public ICommand FilterByGenreCommand { get; }

        public BookViewModel(IBookService bookService)
        {
            _bookService = bookService;
            Books = new BindingList<BookEventArgs>();    

            _bookService.DataChanged += OnModelDataChanged;

            StartupCommand = new RelayCommand(Startup);
            DeleteBookCommand = new RelayCommand(DeleteBook, () => SelectedBook != null);
            AddBookCommand = new RelayCommand(AddBook);
            GetBorrowedBooksCommand = new RelayCommand(GetBorrowedBooks);
            GetAvailableBooksCommand = new RelayCommand(GetAvailableBooks);
            FilterByAuthorCommand = new RelayCommand(FilterByAuthor);
            FilterByGenreCommand = new RelayCommand(FilterByGenre);

            Startup();
        }

        private void Startup()
        {
            _bookService.InvokeDataChanged();
        }

        private void OnModelDataChanged(IEnumerable<Book> books)
        {
            var bookList = books.ToList();
            SyncModelToDTO(bookList);
        }

        private void SyncModelToDTO(List<Book> modelBooks)
        {
            var dtos = new List<BookEventArgs>();
            foreach (var model in modelBooks)
            {
                dtos.Add(new BookEventArgs
                {
                    Id = model.Id,
                    Title = model.Title,
                    Author = model.Author,
                    Genre = model.Genre,
                    IsAvailable = model.IsAvailable,
                    ReaderId = model.ReaderId
                });
            }

            Books = new BindingList<BookEventArgs>(dtos);
        }

        private Book ConvertDTOToModel(BookEventArgs dto) 
        {
            return new Book
            {
                Id = dto.Id,
                Title = dto.Title,
                Author = dto.Author,
                Genre = dto.Genre,
                IsAvailable = dto.IsAvailable,
                ReaderId = dto.ReaderId
            };
        }


        private void AddBook()
        {
            var book = new Book
            {
                Title = NewBookTitle?.Trim(),
                Author = NewBookAuthor?.Trim(),
                Genre = NewBookGenre?.Trim(),
                IsAvailable = true
            };

            _bookService.Add(book);

            NewBookTitle = string.Empty;
            NewBookAuthor = string.Empty;
            NewBookGenre = string.Empty;
        }

        private void DeleteBook() 
        {
            if (SelectedBook != null)
            {
                _bookService.Delete(SelectedBook.Id);
            }
        }

        private void FilterByAuthor() 
        {
            if (string.IsNullOrWhiteSpace(SearchGenre))
            {
                Startup();
                return;
            }

            var filteredBooks = _bookService.FilterByGenre(SearchGenre.Trim());
            SyncModelToDTO(filteredBooks.ToList());
        }

        private void FilterByGenre() 
        {
            if (string.IsNullOrWhiteSpace(SearchAuthor))
            {
                Startup();
                return;
            }

            var filteredBooks = _bookService.FilterByAuthor(SearchAuthor.Trim());
            SyncModelToDTO(filteredBooks.ToList());
        }

        /// <summary>
        /// получить доступные книги
        /// </summary>
        private void GetAvailableBooks()
        {
            var availableBooks = _bookService.GetAvailableBooks();
            SyncModelToDTO(availableBooks.ToList());
        }

        /// <summary>
        /// получить недоступные книги
        /// </summary>
        private void GetBorrowedBooks()
        {
            var borrowedBooks = _bookService.GetBorrowedBooks();
            SyncModelToDTO(borrowedBooks.ToList());
        }

        public void Dispose()
        {
            _bookService.DataChanged -= OnModelDataChanged;
        }

    }
}
