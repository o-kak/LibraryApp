using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    public class LoanService : ILoan
    {
        private IRepository<Reader> ReaderRepository { get; set; }
        private IRepository<Book> BookRepository { get; set; }

        public LoanService(IRepository<Reader> readerRepository, IRepository<Book> bookRepository)
        {
            ReaderRepository = readerRepository;
            BookRepository = bookRepository;
        }

        /// <summary>
        /// выдать книгу
        /// </summary>
        /// <param name="bookId">id книги</param>
        /// <param name="readerId">id читателя</param>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// вернуть книгу
        /// </summary>
        /// <param name="bookId">id книги</param>
        /// <param name="readerId">id читателя</param>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// получить книги, взятые читателем
        /// </summary>
        /// <param name="readerId">id читателя</param>
        /// <returns>список книг</returns>
        public IEnumerable<Book> GetReadersBorrowedBooks(int readerId)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.ReaderId == readerId);
        }
    }
}
