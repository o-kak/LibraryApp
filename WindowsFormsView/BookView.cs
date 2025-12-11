using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsView
{
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
        public void Start()
        {
        }

        public void Redraw(IEnumerable<EventArgs> data)
        {
            var bookEvents = data.OfType<BookEventArgs>();
            _form.Invoke(new Action(() => _form.RedrawBook(bookEvents)));
        }

        public void TriggerFilterByAuthor(string author)
        {
            FilterDataByAuthorEvent?.Invoke(author);
        }

        public void TriggerFilterByGenre(string genre)
        {
            FilterDataByGenreEvent?.Invoke(genre);
        }

        public void TriggerGetAvailableBooks()
        {
            GetAvailableBooksEvent?.Invoke();
        }

        public void TriggerGetBorrowedBooks()
        {
            GetBorrowedBooksEvent?.Invoke();
        }

        public void TriggerAddData(EventArgs data)
        {
            AddDataEvent?.Invoke(data);
        }

        public void TriggerDeleteData(int id)
        {
            DeleteDataEvent?.Invoke(id);
        }

        public void TriggerStartup()
        {
            StartupEvent?.Invoke();
        }
    }
}
