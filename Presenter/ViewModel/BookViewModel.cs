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
        private string _newBookTitle;
        private string _newBookAuthor;
        private string _newBookGenre;
        private BookEventArgs _selectedBook;
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
        public ICommand FilterByAuthorCommand { get; }
        public ICommand FilterByGenreCommand { get; }

        public BookViewModel(IBookService bookService)
        {
            _bookService = bookService;
            Books = new BindingList<BookEventArgs>();

            LoadBooksCommand = new RelayCommand(LoadBooks);
            DeleteBookCommand = new RelayCommand(DeleteBook, () => SelectedBook != null);
            AddBookCommand = new RelayCommand(AddBook, CanAddBook);
            FilterByAuthorCommand = new RelayCommand(FilterByAuthor);
            FilterByGenreCommand = new RelayCommand(FilterByGenre);

            LoadBooks();
        }

        // Конвертация Model -> BookEventArgs (DTO)
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

        // Конвертация BookEventArgs (DTO) -> Model
        private Book ConvertToModel(BookEventArgs dto)
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

        private void AddBook()
        {
            if (!CanAddBook())
            {
                return;
            }

            var newBook = new Book
            {
                Title = NewBookTitle.Trim(),
                Author = NewBookAuthor.Trim(),
                Genre = NewBookGenre.Trim(),
                IsAvailable = true
            };

            _bookService.Add(newBook);

            // Добавляем в коллекцию
            var newDto = ConvertToEventArgs(newBook);
            Books.Add(newDto);

            // Очищаем поля
            NewBookTitle = string.Empty;
            NewBookAuthor = string.Empty;
            NewBookGenre = string.Empty;
        }


        private bool CanAddBook()
        {
            return !string.IsNullOrWhiteSpace(NewBookTitle) &&
                   !string.IsNullOrWhiteSpace(NewBookAuthor) &&
                   !string.IsNullOrWhiteSpace(NewBookGenre);
        }
    }
}
