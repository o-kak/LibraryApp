using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsView
{
    /// <summary>
    /// Представление для управления операциями с книгами в библиотеке
    /// </summary>
    public class BookView: IBookView
    {
        private readonly Form1 _form;

        public event Action<string> FilterDataByAuthorEvent;
        public event Action<string> FilterDataByGenreEvent;
        public event Action GetAvailableBooksEvent;
        public event Action GetBorrowedBooksEvent;
        public event Action<EventArgs> AddDataEvent;
        public event Action<int> DeleteDataEvent;
        public event Action StartupEvent;

        public BookView(Form1 form)
        {
            _form = form;
        }

        /// <summary>
        /// Запускает представление и инициализирует начальное состояние
        /// </summary>
        public void Start()
        {
            TriggerStartup();
        }

        /// <summary>
        /// Перерисовывает список книг на основной форме
        /// </summary>
        /// <param name="data">Коллекция данных о книгах</param>
        public void Redraw(IEnumerable<EventArgs> data)
        {
            var bookEvents = data.OfType<BookEventArgs>();
            _form.Invoke(new Action(() => _form.RedrawBook(bookEvents)));
        }

        /// <summary>
        /// Запускает событие фильтрации книг по автору
        /// </summary>
        /// <param name="author">Автор для фильтрации</param>
        public void TriggerFilterByAuthor(string author)
        {
            FilterDataByAuthorEvent?.Invoke(author);
        }

        /// <summary>
        /// Запускает событие фильтрации книг по жанру
        /// </summary>
        /// <param name="genre">Жанр для фильтрации</param>
        public void TriggerFilterByGenre(string genre)
        {
            FilterDataByGenreEvent?.Invoke(genre);
        }

        /// <summary>
        /// Запускает событие получения списка доступных книг
        /// </summary>
        public void TriggerGetAvailableBooks()
        {
            GetAvailableBooksEvent?.Invoke();
        }

        /// <summary>
        /// Запускает событие получения списка выданных книг
        /// </summary>
        public void TriggerGetBorrowedBooks()
        {
            GetBorrowedBooksEvent?.Invoke();
        }

        /// <summary>
        /// Запускает событие добавления новой книги
        /// </summary>
        /// <param name="data">Данные новой книги</param>
        public void TriggerAddData(EventArgs data)
        {
            AddDataEvent?.Invoke(data);
        }

        /// <summary>
        /// Запускает событие удаления книги
        /// </summary>
        /// <param name="id">Идентификатор удаляемой книги</param>
        public void TriggerDeleteData(int id)
        {
            DeleteDataEvent?.Invoke(id);
        }

        /// <summary>
        /// Запускает событие инициализации при запуске
        /// </summary>
        public void TriggerStartup()
        {
            StartupEvent?.Invoke();
        }
    }
}
