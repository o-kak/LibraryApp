using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IBookView : IEntityView
    {
        /// <summary>
        /// фильтрация книг по автору
        /// </summary>
        event Action<string> FilterDataByAuthorEvent;
        /// <summary>
        /// фильтрация книг по жанру
        /// </summary>
        event Action<string> FilterDataByGenreEvent;
        /// <summary>
        /// получение доступных книг
        /// </summary>
        event Action GetAvailableBooksEvent;
        /// <summary>
        /// получение недоступных книг
        /// </summary>
        event Action GetBorrowedBooksEvent;
    }
}
