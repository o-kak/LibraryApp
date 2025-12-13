using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsView
{
    /// <summary>
    /// Представление для управления операциями выдачи и возврата книг
    /// </summary>
    public class LoanView: ILoanView
    {
        private readonly Form1 _form;
        public event Action<int, int> GiveBookEvent;
        public event Action<int, int> ReturnBookEvent;
        public event Action<int> GetReadersBorrowedBooksEvent;
        public event Action GetAvailableBooksEvent;

        public LoanView(Form1 form) 
        {
            _form = form;
        }

        /// <summary>
        /// Отображает список книг, взятых читателем, в соответствующей форме
        /// </summary>
        /// <param name="books">Коллекция книг, взятых читателем</param>
        public void ShowReadersBorrowedBooks(IEnumerable<EventArgs> books) 
        {
            var bookEvents = books.OfType<BookEventArgs>();
            if (ChangeReaderForm.CurrentInstance != null && !ChangeReaderForm.CurrentInstance.IsDisposed)
            {
                _form.Invoke(new Action(() => ChangeReaderForm.CurrentInstance.UpdateReaderBooks(books)));
            }
        }

        /// <summary>
        /// Отображает список книг в свободном доступе, в соответствующей форме
        /// </summary>
        /// <param name="books">Коллекция доступных книг</param>
        public void ShowAvailableBooks(IEnumerable<EventArgs> books)
        {
            var bookEvents = books.OfType<BookEventArgs>();
            if(ChangeReaderForm.CurrentInstance != null && !ChangeReaderForm.CurrentInstance.IsDisposed)
    {
                _form.Invoke(new Action(() => ChangeReaderForm.CurrentInstance.UpdateAllBooks(books)));
            }
        }

        /// <summary>
        /// Показывает информационное сообщение пользователю
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void ShowMessage(string message)
        {
            _form.Invoke(new Action(() =>
                MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information)));
        }

        /// <summary>
        /// Запускает меню управления выдачей книг (в ВинФорме не реализовано, потому что в нем нет необходимости)
        /// </summary>
        /// <param name="id">Идентификатор читателя</param>
        public void StartLoanMenu(int id)
        {
        }


        /// <summary>
        /// Запускает событие выдачи книги читателю
        /// </summary>
        /// <param name="bookId">Идентификатор книги</param>
        /// <param name="readerId">Идентификатор читателя</param>
        public void TriggerGiveBook(int bookId, int readerId)
        {
            GiveBookEvent?.Invoke(bookId, readerId);
        }

        /// <summary>
        /// Запускает событие возврата книги читателю
        /// </summary>
        /// <param name="bookId">Идентификатор книги</param>
        /// <param name="readerId">Идентификатор читателя</param>
        public void TriggerReturnBook(int bookId, int readerId)
        {
            ReturnBookEvent?.Invoke(bookId, readerId);
        }

        /// <summary>
        /// Запускает событие получения списка книг, взятых указанным читателем
        /// </summary>
        /// <param name="readerId">Идентификатор читателя</param>
        public void TriggerGetReadersBorrowedBooks(int readerId)
        {
            GetReadersBorrowedBooksEvent?.Invoke(readerId);
        }

        /// <summary>
        /// Запускает событие получения списка всех доступных книг
        /// </summary>
        public void TriggerGetAvailableBooks()
        {
            GetAvailableBooksEvent?.Invoke();
        }

    }
}
