using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class LibraryManager
    {
        public List<Reader> Readers;
        public List<Book> Books;
        public LibraryManager()
        {
            Readers = new List<Reader>();
            Books = new List<Book>();
        }

        public void AddReader(string name, string address)
        {
            int id = Readers.Count + 1;
            Reader reader = new Reader(name, address, id);
            Readers.Add(reader);
        }

        public void AddBook(string title, string author, string genre)
        {
            Book book = new Book(title, author, genre);
            Books.Add(book);
        }

        public void DeleteReader(Reader reader)
        {
            Readers.Remove(reader);
        }

        public void DeleteBook(Book book)
        {
            Books.Remove(book);
        }
    }
}
