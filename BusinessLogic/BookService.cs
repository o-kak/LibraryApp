using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;

namespace BusinessLogic
{
    public class BookService : IBookService
    {
        private IRepository<Book> BookRepository { get; set; }

        public BookService(IRepository<Book> bookRepository)
        {
            BookRepository = bookRepository;
        }

        /// <summary>
        /// добавить книгу
        /// </summary>
        /// <param name="title">название</param>
        /// <param name="author">автор</param>
        /// <param name="genre">жанр</param>
        public void AddBook(string title, string author, string genre)
        {
            Book book = new Book(title, author, genre);
            BookRepository.Add(book);
        }

        /// <summary>
        /// удалить книгу
        /// </summary>
        /// <param name="bookId">id книги</param>
        public void DeleteBook(int bookId) => BookRepository.Delete(bookId);

        /// <summary>
        /// получить все книги
        /// </summary>
        /// <returns>список книг</returns>
        public IEnumerable<Book> GetAllBooks()
        {
            return BookRepository.ReadAll();
        }

    }
}
