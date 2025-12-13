using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsView
{
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

        public void ShowReadersBorrowedBooks(IEnumerable<EventArgs> books) 
        {
            var bookEvents = books.OfType<BookEventArgs>();
            if (ChangeReaderForm.CurrentInstance != null && !ChangeReaderForm.CurrentInstance.IsDisposed)
            {
                _form.Invoke(new Action(() => ChangeReaderForm.CurrentInstance.UpdateReaderBooks(books)));
            }
        }
        public void ShowAvailableBooks(IEnumerable<EventArgs> books)
        {
            var bookEvents = books.OfType<BookEventArgs>();
            if(ChangeReaderForm.CurrentInstance != null && !ChangeReaderForm.CurrentInstance.IsDisposed)
    {
                _form.Invoke(new Action(() => ChangeReaderForm.CurrentInstance.UpdateAllBooks(books)));
            }
        }
        public void ShowMessage(string message)
        {
            _form.Invoke(new Action(() =>
                MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information)));
        }

        public void StartLoanMenu(int id)
        {
            //_form.Invoke(new Action(() => _form.ShowLoanMenuDialog(id)));
        }

        public void TriggerGiveBook(int bookId, int readerId)
        {
            GiveBookEvent?.Invoke(bookId, readerId);
        }

        public void TriggerReturnBook(int bookId, int readerId)
        {
            ReturnBookEvent?.Invoke(bookId, readerId);
        }

        public void TriggerGetReadersBorrowedBooks(int readerId)
        {
            GetReadersBorrowedBooksEvent?.Invoke(readerId);
        }

        public void TriggerGetAvailableBooks()
        {
            GetAvailableBooksEvent?.Invoke();
        }

    }
}
