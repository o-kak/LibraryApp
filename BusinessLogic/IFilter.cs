using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    internal interface IFilter<T>
    {
        /// <summary>
        /// фильтрация по критерию
        /// </summary>
        /// <param name="criteria">критерий</param>
        /// <returns></returns>
        IEnumerable<T> Filter(string criteria);
    }
}
