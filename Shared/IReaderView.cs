using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IReaderView : IEntityView
    {
        event Action<EventArgs> UpdateDataEvent;
        event Action<int> ReadByIdEvent;
        event Action GetAvailableBooksEvent;
        event Action<int> GetBorrowedBooksEvent;
        void ShowReaderProfile(ReaderEventArgs reader);
        void ShowBorrowedBooks(IEnumerable<EventArgs> books);
    }
}
