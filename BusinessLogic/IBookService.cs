using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    public interface IBookService : IModel<Book>
    {
        IEnumerable<Book> FilterByAuthor(string author);
        IEnumerable<Book> FilterByGenre(string genre);
        IEnumerable<Book> GetBorrowedBooks();
        IEnumerable<Book> GetAvailableBooks();

    }
}
