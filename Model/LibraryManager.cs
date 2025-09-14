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

        public void AddReader(string name, string address)
        {
            int id = _readers.Count + 1;
            Reader reader = new Reader(name, address, id);
            _readers.Add(reader);
        }

        public void AddBook(string title, string author, string genre)
        {
            Book book = new Book(title, author, genre);
            _books.Add(book);
        }

        public void DeleteReader(Reader reader) => _readers.Remove(reader);

        public void DeleteBook(Book book) => _books.Remove(book);

        public void GiveBook(Book book, Reader reader)
        {
            if (!book.IsAvailable)
                throw new InvalidOperationException("Этой книги нет в фонде!");
            
            reader.BorrowBook(book);
            book.UpdateAvailability(false);
        }

        public void ReturnBook(Book book, Reader reader)
        {
            if (!reader.ReturnBook(book))
                throw new InvalidOperationException("У читателя нет этой книги!");

            book.UpdateAvailability(true);
        }

        public IEnumerable<Book> GetAvailableBooks()
        {
            return _books.Where(book => book.IsAvailable);
        }

        public IEnumerable<Book> GetBorrowedBooks()
        {
            return _books.Where(book =>  !book.IsAvailable);
        }
    }
}
