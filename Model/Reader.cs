using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Reader
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int ID { get; set; }
        public List<Book> BooksBorrowed { get; set; } = new List<Book>();

        public Reader(string name, string address, int id)
        {
            Name = name;
            Address = address;
            ID = id;
        }

        /// <summary>
        /// изменение имени
        /// </summary>
        /// <param name="name">новое имя</param>
        public void UpdateName(string name) => Name = name;

        /// <summary>
        /// изменение адреса
        /// </summary>
        /// <param name="address">новый адрес</param>
        public void UpdateAddress(string address) => Address = address;

        /// <summary>
        /// взять книгу из библиотеки - добавляет книгу в коллекцию BooksBorrowed
        /// </summary>
        /// <param name="book">взятая книга</param>
        public void BorrowBook(Book book) => BooksBorrowed.Add(book);
        
        /// <summary>
        /// вернуть книгу - убирает книгу из коллекции, если она там есть
        /// </summary>
        /// <param name="book">выбранная книга</param>
        public bool ReturnBook(Book book)
        {
            if (BooksBorrowed.Contains(book))
            {
                BooksBorrowed.Remove(book);
                return true;
            }
            return false;
        }
    }
}
