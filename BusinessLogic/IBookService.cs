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
        /// <summary>
        /// Фильтрация книг по автору
        /// </summary>
        /// <param name="author">автор</param>
        /// <returns>список отфильтрованных книг</returns>
        IEnumerable<Book> FilterByAuthor(string author);
        /// <summary>
        /// Фильтрация книг по жанру
        /// </summary>
        /// <param name="genre">жанр</param>
        /// <returns>список отфильтрованных книг</returns>
        IEnumerable<Book> FilterByGenre(string genre);
        /// <summary>
        /// Получить недоступные книги
        /// </summary>
        /// <returns>Список недоступных книг</returns>
        IEnumerable<Book> GetBorrowedBooks();
        /// <summary>
        /// Получить доступные книги
        /// </summary>
        /// <returns>список доступных книг</returns>
        IEnumerable<Book> GetAvailableBooks();

    }
}
