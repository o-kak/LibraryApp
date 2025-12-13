using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsView
{
    /// <summary>
    /// Представление для управления операциями с читателями библиотеки
    /// </summary>
    public class ReaderView: IReaderView
    {
        private readonly Form1 _form;

        public event Action<EventArgs> AddDataEvent;
        public event Action<int> DeleteDataEvent;
        public event Action<EventArgs> UpdateDataEvent;
        public event Action<int> ReadByIdEvent;
        public event Action GetAvailableBooksEvent;
        public event Action<int> GetBorrowedBooksEvent;
        public event Action StartupEvent;

        public ReaderView(Form1 form) 
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
        /// Перерисовывает список читателей на основной форме
        /// </summary>
        /// <param name="data">Коллекция данных о читателях</param>
        public void Redraw(IEnumerable<EventArgs> data)
        {

            var readerEvents = data.OfType<ReaderEventArgs>();
            _form.Invoke(new Action(() => _form.RedrawReader(readerEvents)));
        }

        /// <summary>
        /// Отображает окно с профилем читателя(ChangeReaderForm)
        /// </summary>
        /// <param name="reader">Данные читателя для отображения</param>
        public void ShowReaderProfile(ReaderEventArgs reader)
        {
            _form.Invoke(new Action(() => _form.ShowReaderProfileDialog(reader)));
        }

        /// <summary>
        /// Отображает диалог со списком книг, взятых читателем
        /// </summary>
        /// <param name="books">Коллекция книг, взятых читателем</param>
        public void ShowBorrowedBooks(IEnumerable<EventArgs> books)
        {
            var bookEvents = books.OfType<BookEventArgs>();
            _form.Invoke(new Action(() => _form.ShowBorrowedBooksDialog(bookEvents)));
        }

        /// <summary>
        /// Запускает событие добавления нового читателя
        /// </summary>
        /// <param name="data">Данные нового читателя</param>
        public void TriggerAddData(EventArgs data)
        {
            AddDataEvent?.Invoke(data);
        }

        /// <summary>
        /// Запускает событие удаления читателя
        /// </summary>
        /// <param name="id">Идентификатор удаляемого читателя</param>
        public void TriggerDeleteData(int id)
        {
            DeleteDataEvent?.Invoke(id);
        }

        /// <summary>
        /// Запускает событие обновления данных читателя
        /// </summary>
        /// <param name="data">Обновленные данные читателя</param>
        public void TriggerUpdateData(EventArgs data)
        {
            UpdateDataEvent?.Invoke(data);
        }

        /// <summary>
        /// Запускает событие получения данных читателя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор читателя</param>
        public void TriggerReadById(int id)
        {
            ReadByIdEvent?.Invoke(id);
        }

        /// <summary>
        /// Запускает событие получения списка книг, взятых указанным читателем
        /// </summary>
        /// <param name="readerId">Идентификатор читателя</param>
        public void TriggerGetBorrowedBooks(int readerId)
        {
            GetBorrowedBooksEvent?.Invoke(readerId);
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
