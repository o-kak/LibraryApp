using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Delete(int id);
        void Update(T entity);
        T ReadById(int id);
        IEnumerable<T> ReadAll();
    }
}
