using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    public interface ILoan
    {
        void GiveBook(int bookId, int readerId);
        void ReturnBook(int bookId, int readerId);
        IEnumerable<Book> GetReadersBorrowedBooks(int readerId);
    }
}
