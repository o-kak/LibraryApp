using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;

namespace BusinessLogic
{
    public class BookService : IModel<Book>
    {
        private IRepository<Book> BookRepository { get; set; }
        public event Action<IEnumerable<Book>> DataChanged;

        private BookAuthorFilter AuthorFilter { get; set; }
        private BookGenreFilter GenreFilter { get; set; }
        public BookService(IRepository<Book> bookRepository, BookAuthorFilter authorFilter, BookGenreFilter genreFilter)
        {
            BookRepository = bookRepository;
            AuthorFilter = authorFilter;
            GenreFilter = genreFilter;
        }

        /// <summary>
        /// добавить книгу
        /// </summary>
        /// <param name="title">название</param>
        /// <param name="author">автор</param>
        /// <param name="genre">жанр</param>
        public void Add(Book book)
        {
            BookRepository.Add(book);
            InvokeDataChanged();
            
        }

        /// <summary>
        /// удалить книгу
        /// </summary>
        /// <param name="bookId">id книги</param>
        public void Delete(int bookId)
        {
            BookRepository.Delete(bookId);
            InvokeDataChanged();
        }

        public void InvokeDataChanged()
        {
            DataChanged?.Invoke(new List<Book>(BookRepository.ReadAll()));
        }

        public IEnumerable<Book> FilterByAuthor(string author)
        {
            List<Book> filteredBooks = AuthorFilter.Filter(author).ToList();
            return filteredBooks;
        }

        public IEnumerable<Book> FilterByGenre(string genre)
        {
            List<Book> filteredBooks = GenreFilter.Filter(genre).ToList();
            return filteredBooks;
        }

        public IEnumerable<Book> GetBorrowedBooks()
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => !book.IsAvailable);
        }

        public IEnumerable<Book> GetAvailableBooks()
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.IsAvailable);
        }

    }
}
