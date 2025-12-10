using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IReaderView : IEntityView
    {
        /// <summary>
        /// обновление данных
        /// </summary>
        event Action<EventArgs> UpdateDataEvent;
        /// <summary>
        /// открыть профиль пользователя
        /// </summary>
        event Action<int> ReadByIdEvent;
        /// <summary>
        /// получить доступные книги
        /// </summary>
        event Action GetAvailableBooksEvent;
        /// <summary>
        /// получить книги на руках у читателя
        /// </summary>
        event Action<int> GetBorrowedBooksEvent;
        /// <summary>
        /// UI профиля читателя
        /// </summary>
        /// <param name="reader">читатель</param>
        void ShowReaderProfile(ReaderEventArgs reader);
        /// <summary>
        /// показать книги на рукаху читателя
        /// </summary>
        /// <param name="books">список книг</param>
        void ShowBorrowedBooks(IEnumerable<EventArgs> books);
    }
}
