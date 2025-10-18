using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    public class LibraryManager
    {
        private readonly IRepository<Reader> _readerRepository;
        private readonly IRepository<Book> _bookRepository;

        public LibraryManager()
        {
            AppDbContext context = new AppDbContext();
            _readerRepository = new EntityRepository<Reader>(context);
            _bookRepository = new EntityRepository<Book>(context);
        }

        /// <summary>
        /// добавить читателя
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="address">адрес</param>
        public void AddReader(string name, string address)
        {
            Reader reader = new Reader(name, address);
            _readerRepository.Add(reader);
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
            _bookRepository.Add(book);
        }

        /// <summary>
        /// удалить читателя из списка
        /// </summary>
        /// <param name="reader">читатель</param>
        public void DeleteReader(int readerId)
        {
            var readerToDelete = _readerRepository.ReadById(readerId);

            if (readerToDelete != null)
            {
                var booksToReturn = readerToDelete.BooksBorrowed?.ToList();

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

                _readerRepository.Delete(readerId);
            }
        }

        /// <summary>
        /// удалить книгу из списка
        /// </summary>
        /// <param name="book">книга</param>
        public void DeleteBook(int bookId) => _bookRepository.Delete(bookId);

        /// <summary>
        /// дать книгу читателю: вызывает у читателя метод BorrowBook, меняет статус доступности книги на ложь
        /// </summary>
        /// <param name="book">книга</param>
        /// <param name="reader">читатель</param>
        /// <exception cref="InvalidOperationException">книга недоступна</exception>
        public void GiveBook(int bookId, int readerId)
        {
            var book = _bookRepository.ReadById(bookId);
            var reader = _readerRepository.ReadById(readerId);

            if (book == null)
                throw new InvalidOperationException("Книга не найдена!");

            if (reader == null)
                throw new InvalidOperationException("Читатель не найден!");

            if (!book.IsAvailable)
                throw new InvalidOperationException("Этой книги нет в фонде!");

            reader.BorrowBook(book);
            book.UpdateAvailability(false);
            book.ReaderId = readerId;

            _bookRepository.Update(book);
            _readerRepository.Update(reader);
        }

        /// <summary>
        /// вернуть книгу: вызывает у читателя метод ReturnBook, меняет статус доступности книги на правду
        /// </summary>
        /// <param name="book">книга</param>
        /// <param name="reader">читатель</param>
        /// <exception cref="InvalidOperationException">ошибка если список книг пустой</exception>
        public void ReturnBook(int bookId, int readerId)
        {
            var book = _bookRepository.ReadById(bookId);
            var reader = _readerRepository.ReadById(readerId);

            if (book == null)
                throw new InvalidOperationException("Книга не найдена!");

            if (reader == null)
                throw new InvalidOperationException("Читатель не найден!");

            if (!reader.ReturnBook(book))
                throw new InvalidOperationException("У читателя нет этой книги!");

            book.UpdateAvailability(true);
            book.ReaderId = null;

            _bookRepository.Update(book);
            _readerRepository.Update(reader);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.ReadAll();
        }

        public IEnumerable<Reader> GetAllReaders()
        {
            return _readerRepository.ReadAll();
        }

        public Reader GetReader(int readerId)
        {
            return _readerRepository.ReadById(readerId);
        }

        /// <summary>
        /// вывод книг, которые есть в фонде
        /// </summary>
        /// <returns>коллекция доступных книг</returns>
        public IEnumerable<Book> GetAvailableBooks()
        {
            List<Book> allBooks = _bookRepository.ReadAll().ToList(); 
            return allBooks.Where(book => book.IsAvailable);
        }

        public IEnumerable<Book> GetReadersBorrowedBooks(int readerId)
        {
            List<Book> allBooks = _bookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.ReaderId == readerId);
        }

        /// <summary>
        /// вывод книг, которых нет в фонде
        /// </summary>
        /// <returns>коллекция недоступных книг</returns>
        public IEnumerable<Book> GetBorrowedBooks()
        {
            List<Book> allBooks = _bookRepository.ReadAll().ToList();
            return allBooks.Where(book => !book.IsAvailable);
        }

        /// <summary>
        /// фильтрация книг по жанру
        /// </summary>
        /// <param name="genre">жанр</param>
        /// <returns>коллекция отфильтрованных книг</returns>
        public IEnumerable<Book> FilterBooksByGenre(string genre)
        {
            List<Book> allBooks = _bookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Genre == genre);
        }

        /// <summary>
        /// фильтрация книг по автору
        /// </summary>
        /// <param name="author">автор</param>
        /// <returns>коллекция отфильтрованных книг</returns>
        public IEnumerable<Book> FilterBooksByAuthor(string author)
        {
            List<Book> allBooks = _bookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Author == author);
        }
    }
}
