using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    public class LoanService
    {
        private IRepository<Reader> ReaderRepository { get; set; }
        private IRepository<Book> BookRepository { get; set; }

        public LoanService(IRepository<Reader> readerRepository, IRepository<Book> bookRepository)
        {
            ReaderRepository = readerRepository;
            BookRepository = bookRepository;
        }

        public void GiveBook(int bookId, int readerId)
        {
            var book = BookRepository.ReadById(bookId);
            var reader = ReaderRepository.ReadById(readerId);

            if (book == null)
                throw new InvalidOperationException("Книга не найдена!");

            if (reader == null)
                throw new InvalidOperationException("Читатель не найден!");

            if (!book.IsAvailable)
                throw new InvalidOperationException("Этой книги нет в фонде!");


            book.UpdateAvailability(false);
            book.ReaderId = readerId;

            BookRepository.Update(book);
            ReaderRepository.Update(reader);
        }

        public void ReturnBook(int bookId, int readerId)
        {
            var book = BookRepository.ReadById(bookId);
            var reader = ReaderRepository.ReadById(readerId);

            if (book == null)
                throw new InvalidOperationException("Книга не найдена!");

            if (reader == null)
                throw new InvalidOperationException("Читатель не найден!");

            book.UpdateAvailability(true);
            book.ReaderId = null;

            BookRepository.Update(book);
            ReaderRepository.Update(reader);
        }

        public IEnumerable<Book> GetReadersBorrowedBooks(int readerId)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.ReaderId == readerId);
        }

        public IEnumerable<Book> GetAvailableBooks()
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.IsAvailable);
        }

        public IEnumerable<Book> GetBorrowedBooks()
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => !book.IsAvailable);
        }
    }
}
