using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IModel<T> where T : class
    {
        event Action<IEnumerable<T>> DataChanged;

        void Delete(int id);
        void Add(T entity);
        void Update(T entity);
        void InvokeDataChanged();
    }
}
