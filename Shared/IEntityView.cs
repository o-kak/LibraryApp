using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IEntityView : IView
    {
        /// <summary>
        /// добавление данных пользователем
        /// </summary>
        event Action<EventArgs> AddDataEvent;
        /// <summary>
        /// удаление данных пользователем
        /// </summary>
        event Action<int> DeleteDataEvent;
        /// <summary>
        /// Перерисовка UI
        /// </summary>
        /// <param name="data"></param>
        void Redraw(IEnumerable<EventArgs> data);
    }
}
