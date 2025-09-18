using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LibraryManager
    {
        private readonly List<Reader> _readers = new List<Reader>();
        private readonly List<Book> _books = new List<Book>();

        public IEnumerable<Book> Books => _books;
        public IEnumerable<Reader> Readers => _readers;

        public LibraryManager()
        {
            _readers = new List<Reader>();
            _books = new List<Book>();
        }
        
        /// <summary>
        /// добавить читателя
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="address">адрес</param>
        public void AddReader(string name, string address)
        {
            int id = _readers.Count + 1;
            Reader reader = new Reader(name, address, id);
            _readers.Add(reader);
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
            _books.Add(book);
        }

        /// <summary>
        /// удалить читателя из списка
        /// </summary>
        /// <param name="reader">читатель</param>
        public void DeleteReader(Reader reader) => _readers.Remove(reader);

        /// <summary>
        /// удалить книгу из списка
        /// </summary>
        /// <param name="book">книга</param>
        public void DeleteBook(Book book) => _books.Remove(book);

        /// <summary>
        /// дать книгу читателю: вызывает у читателя метод BorrowBook, меняет статус доступности книги на ложь
        /// </summary>
        /// <param name="book">книга</param>
        /// <param name="reader">читатель</param>
        /// <exception cref="InvalidOperationException">книга недоступна</exception>
        public void GiveBook(Book book, Reader reader)
        {
            if (!book.IsAvailable)
                throw new InvalidOperationException("Этой книги нет в фонде!");
            
            reader.BorrowBook(book);
            book.UpdateAvailability(false);
        }

        /// <summary>
        /// вернуть книгу: вызывает у читателя метод ReturnBook, меняет статус доступности книги на правду
        /// </summary>
        /// <param name="book">книга</param>
        /// <param name="reader">читатель</param>
        /// <exception cref="InvalidOperationException">ошибка если список книг пустой</exception>
        public void ReturnBook(Book book, Reader reader)
        {
            if (!reader.ReturnBook(book))
                throw new InvalidOperationException("У читателя нет этой книги!");

            book.UpdateAvailability(true);
        }

        /// <summary>
        /// вывод книг, которые есть в фонде
        /// </summary>
        /// <returns>коллекция доступных книг</returns>
        public IEnumerable<Book> GetAvailableBooks()
        {
            return _books.Where(book => book.IsAvailable);
        }

        /// <summary>
        /// вывод книг, которых нет в фонде
        /// </summary>
        /// <returns>коллекция недоступных книг</returns>
        public IEnumerable<Book> GetBorrowedBooks()
        {
            return _books.Where(book =>  !book.IsAvailable);
        }

        /// <summary>
        /// фильтрация книг по жанру
        /// </summary>
        /// <param name="genre">жанр</param>
        /// <returns>коллекция отфильтрованных книг</returns>
        public IEnumerable<Book> FilterBooksByGenre(string genre)
        {
            return _books.Where(book => book.Genre == genre);
        }

        /// <summary>
        /// фильтрация книг по автору
        /// </summary>
        /// <param name="author">автор</param>
        /// <returns>коллекция отфильтрованных книг</returns>
        public IEnumerable<Book> FilterBooksByAuthor(string author)
        {
            return _books.Where(book => book.Author == author);
        }
    }
}
