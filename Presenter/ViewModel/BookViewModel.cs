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
using System.Windows.Input;

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
            set { _newBookTitle = value; OnPropertyChanged(); }
        }

        public string NewBookAuthor
        {
            get => _newBookAuthor;
            set { _newBookAuthor = value; OnPropertyChanged(); }
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

        public ICommand LoadBooksCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand AddBookCommand { get; }

        public ICommand GetBorrowedBooks { get; }
        public ICommand GetAvailableBooks { get; }
        public ICommand FilterByAuthorCommand { get; }
        public ICommand FilterByGenreCommand { get; }

        public BookViewModel(IBookService bookService)
        {
            _bookService = bookService;
            Books = new BindingList<BookEventArgs>();

            LoadBooksCommand = new RelayCommand(LoadBooks);
            DeleteBookCommand = new RelayCommand(DeleteBook, () => SelectedBook != null);
            AddBookCommand = new RelayCommand(AddBook);
            FilterByAuthorCommand = new RelayCommand(FilterByAuthor);
            FilterByGenreCommand = new RelayCommand(FilterByGenre);

            LoadBooks();
        }


        private BookEventArgs ConvertToEventArgs(Book model)
        {
            return new BookEventArgs
            {
                Id = model.Id,
                Title = model.Title,
                Author = model.Author,
                Genre = model.Genre,
                IsAvailable = model.IsAvailable,
                ReaderId = model.ReaderId
            };
        }

        private void LoadBooks() 
        {
            var availableBooks = _bookService.GetAvailableBooks();
            var borrowedBooks = _bookService.GetBorrowedBooks();
            var allBooks = availableBooks.Concat(borrowedBooks);

            Books.Clear();
            foreach (var book in allBooks)
            {
                Books.Add(ConvertToEventArgs(book));
            }

        }

        private void AddBook()
        {
            Book book = new Book();
            book.Id = 0;
            book.Title = NewBookTitle.Trim();
            book.Author = NewBookAuthor.Trim();
            book.Genre = NewBookGenre.Trim();
            book.IsAvailable = true;
            _bookService.Add(book);

            var newDto = ConvertToEventArgs(book);
            Books.Add(newDto);

            NewBookTitle = string.Empty;
            NewBookAuthor = string.Empty;
            NewBookGenre = string.Empty;
        }

        private void DeleteBook() 
        {
            if (SelectedBook != null) 
            {
                _bookService.Delete(SelectedBook.Id);
                Books.Remove(SelectedBook);
                SelectedBook = null;
            }

        }

        private void FilterByAuthor() 
        {
            if (!string.IsNullOrEmpty(SearchAuthor))
            {
                var modelBooks = _bookService.FilterByAuthor(SearchAuthor);
                Books.Clear();
                foreach (var book in modelBooks)
                {
                    Books.Add(ConvertToEventArgs(book));
                }
            }
        }

        private void FilterByGenre() 
        {
            if (!string.IsNullOrEmpty(SearchGenre))
            {
                var modelBooks = _bookService.FilterByGenre(SearchGenre);
                Books.Clear();
                foreach (var book in modelBooks)
                {
                    Books.Add(ConvertToEventArgs(book));
                }
            }

        }

    }
}
