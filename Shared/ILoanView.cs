using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ILoanView
    {
        /// <summary>
        /// выдача книг
        /// </summary>
        event Action<int, int> GiveBookEvent;
        /// <summary>
        /// возврат книг
        /// </summary>
        event Action<int, int> ReturnBookEvent;
        /// <summary>
        /// получить книги на руках у читателя
        /// </summary>
        event Action<int> GetReadersBorrowedBooksEvent;
        /// <summary>
        /// Получить доступные книги
        /// </summary>
        event Action GetAvailableBooksEvent;
        /// <summary>
        /// показать книги на руках у читателя
        /// </summary>
        /// <param name="books">список книг</param>
        void ShowReadersBorrowedBooks(IEnumerable<EventArgs> books);
        /// <summary>
        /// показать доступные книги
        /// </summary>
        /// <param name="books">список книг</param>
        void ShowAvailableBooks(IEnumerable<EventArgs> books);
        /// <summary>
        /// вывод сообщения
        /// </summary>
        /// <param name="message">сообщение</param>
        void ShowMessage(string message);
        /// <summary>
        /// запуск меню займов
        /// </summary>
        /// <param name="id"></param>
        void StartLoanMenu(int id);
    }
}
