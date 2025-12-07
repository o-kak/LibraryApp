using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IReaderView : IView
    {
        event Action<EventArgs> UpdateDataEvent;
        event Action<int> ReadByIdEvent;
        void ShowReaderProfile(ReaderEventArgs reader);
    }
}
