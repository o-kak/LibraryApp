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
        IEnumerable<T> Filter(string criteria);
    }
}
