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
        /// <summary>
        /// Выдача книг.
        /// </summary>
        /// <param name="bookId">Id книги</param>
        /// <param name="readerId">Id читатаеля</param>
        void GiveBook(int bookId, int readerId);
        /// <summary>
        /// Возврат книг.
        /// </summary>
        /// <param name="bookId">Id книги</param>
        /// <param name="readerId">Id читателя</param>
        void ReturnBook(int bookId, int readerId);
        /// <summary>
        /// Просмотр книг на руках у читателя
        /// </summary>
        /// <param name="readerId">Id читателя</param>
        /// <returns>список книг у читателя</returns>
        IEnumerable<Book> GetReadersBorrowedBooks(int readerId);
    }
}
