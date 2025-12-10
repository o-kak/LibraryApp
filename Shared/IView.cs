using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IView
    {
        /// <summary>
        /// собыие запуска
        /// </summary>
        event Action StartupEvent;
        /// <summary>
        /// запуск
        /// </summary>
        void Start();
    }
}
