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
        private IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(string title, string author, string genre)
        {
            Book book = new Book(title, author, genre);
            _bookRepository.Add(book);
        }

        public void DeleteBook(int bookId) => _bookRepository.Delete(bookId);

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.ReadAll();
        }


    }
}
