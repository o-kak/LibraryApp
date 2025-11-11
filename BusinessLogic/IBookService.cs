using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    public interface IBookService
    {
        void AddBook(string title, string author, string genre);
        void DeleteBook(int bookId);
        IEnumerable<Book> GetAllBooks();
    }
}
