using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface ILoanView
    {
        event Action<int, int> GiveBookEvent;
        event Action<int, int> ReturnBookEvent;
        event Action<int> GetReadersBorrowedBooksEvent;
        event Action GetAvailableBooksEvent;
        void ShowReadersBorrowedBooks(IEnumerable<EventArgs> books);
        void ShowAvailableBooks(IEnumerable<EventArgs> books);
        void ShowMessage(string message);
        void StartLoanMenu(int id);
    }
}
