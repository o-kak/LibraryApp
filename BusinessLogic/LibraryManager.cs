using DataAccessLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class LibraryManager
    {
        private IRepository<Reader> ReaderRepository {  get; set; }
        private IRepository<Book> BookRepository { get; set; }

        public LibraryManager(IRepository<Reader> readerRepository, IRepository<Book> bookRepository)
        {
            ReaderRepository = readerRepository;
            BookRepository = bookRepository;
        }

        /// <summary>
        /// добавить читателя
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="address">адрес</param>
        public void AddReader(string name, string address)
        {
            Reader reader = new Reader(name, address);
            ReaderRepository.Add(reader);
        }

        /// <summary>
        /// добавить книгу
        /// </summary>
        /// <param name="title">навание</param>
        /// <param name="author">автор</param>
        /// <param name="genre">жанр</param>
        public void AddBook(string title, string author, string genre)
        {
            Book book = new Book(title, author, genre);
            BookRepository.Add(book);
        }

        /// <summary>
        /// удалить читателя
        /// </summary>
        /// <param name="readerId">id читателя</param>
        public void DeleteReader(int readerId)
        {
            var readerToDelete = ReaderRepository.ReadById(readerId);

            if (readerToDelete != null)
            {
                var booksToReturn = GetReadersBorrowedBooks(readerToDelete.Id).ToList();

                if (booksToReturn != null && booksToReturn.Any())
                {
                    foreach (var book in booksToReturn)
                    {
                        if (book != null)
                        {
                            ReturnBook(book.Id, readerToDelete.Id);
                        }
                    }
                }

                ReaderRepository.Delete(readerId);
            }
        }

        /// <summary>
        /// удалить книгу
        /// </summary>
        /// <param name="bookId">id книги</param>
        public void DeleteBook(int bookId) => BookRepository.Delete(bookId);

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
       /// вернуть кигу
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
        /// список всех книг
        /// </summary>
        /// <returns>список книг</returns>
        public IEnumerable<Book> GetAllBooks()
        {
            return BookRepository.ReadAll();
        }

        /// <summary>
        /// списк всех читателей
        /// </summary>
        /// <returns>список читателей</returns>
        public IEnumerable<Reader> GetAllReaders()
        {
            return ReaderRepository.ReadAll();
        }

        /// <summary>
        /// получить читателя по айди
        /// </summary>
        /// <param name="readerId">id читателя</param>
        /// <returns>читатель</returns>
        public Reader GetReader(int readerId)
        {
            return ReaderRepository.ReadById(readerId);
        }

        /// <summary>
        /// вывод книг, которые есть в фонде
        /// </summary>
        /// <returns>коллекция доступных книг</returns>
        public IEnumerable<Book> GetAvailableBooks()
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList(); 
            return allBooks.Where(book => book.IsAvailable);
        }

        /// <summary>
        /// книги у читателя
        /// </summary>
        /// <param name="readerId">id читателя</param>
        /// <returns>список книг</returns>
        public IEnumerable<Book> GetReadersBorrowedBooks(int readerId)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.ReaderId == readerId);
        }

        /// <summary>
        /// вывод книг, которых нет в фонде
        /// </summary>
        /// <returns>коллекция недоступных книг</returns>
        public IEnumerable<Book> GetBorrowedBooks()
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => !book.IsAvailable);
        }

        /// <summary>
        /// фильтрация книг по жанру
        /// </summary>
        /// <param name="genre">жанр</param>
        /// <returns>коллекция отфильтрованных книг</returns>
        public IEnumerable<Book> FilterBooksByGenre(string genre)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Genre == genre);
        }

        /// <summary>
        /// фильтрация книг по автору
        /// </summary>
        /// <param name="author">автор</param>
        /// <returns>коллекция отфильтрованных книг</returns>
        public IEnumerable<Book> FilterBooksByAuthor(string author)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Author == author);
        }
    }
}
