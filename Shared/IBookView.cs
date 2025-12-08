using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IBookView : IEntityView
    {
        event Action<string> FilterDataByAuthorEvent;
        event Action<string> FilterDataByGenreEvent;
        event Action GetAvailableBooksEvent;
        event Action GetBorrowedBooksEvent;
    }
}
