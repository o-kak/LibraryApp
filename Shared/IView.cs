using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IView
    {
        event Action<EventArgs> AddDataEvent;
        event Action<int> DeleteDataEvent;
        void Redraw(IEnumerable<EventArgs> data);
    }
}
