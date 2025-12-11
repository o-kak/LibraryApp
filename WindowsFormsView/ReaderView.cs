using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsView
{
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
        public void Start() 
        {

        }

        public void Redraw(IEnumerable<EventArgs> data)
        {
            var readerEvents = data.OfType<ReaderEventArgs>();
            _form.Invoke(new Action(() => _form.RedrawReader(readerEvents)));
        }
        public void ShowReaderProfile(ReaderEventArgs reader)
        {
            _form.Invoke(new Action(() => _form.ShowReaderProfileDialog(reader)));
        }

        public void ShowBorrowedBooks(IEnumerable<EventArgs> books)
        {
            var bookEvents = books.OfType<BookEventArgs>();
            _form.Invoke(new Action(() => _form.ShowBorrowedBooksDialog(bookEvents)));
        }

        public void TriggerAddData(EventArgs data)
        {
            AddDataEvent?.Invoke(data);
        }

        public void TriggerDeleteData(int id)
        {
            DeleteDataEvent?.Invoke(id);
        }

        public void TriggerUpdateData(EventArgs data)
        {
            UpdateDataEvent?.Invoke(data);
        }

        public void TriggerReadById(int id)
        {
            ReadByIdEvent?.Invoke(id);
        }

        public void TriggerGetBorrowedBooks(int readerId)
        {
            GetBorrowedBooksEvent?.Invoke(readerId);
        }

        public void TriggerStartup()
        {
            StartupEvent?.Invoke();
        }

    }
}
