using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;

namespace BusinessLogic
{
    public class BookService
    {
        private IRepository<Book> BookRepository { get; set; }

        public BookService(IRepository<Book> bookRepository)
        {
            BookRepository = bookRepository;
        }

        public void AddBook(string title, string author, string genre)
        {
            Book book = new Book(title, author, genre);
            BookRepository.Add(book);
        }

        public void DeleteBook(int bookId) => BookRepository.Delete(bookId);

        public IEnumerable<Book> GetAllBooks()
        {
            return BookRepository.ReadAll();
        }

    }
}
