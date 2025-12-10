using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IModel<T> where T : class
    {
        /// <summary>
        /// Событие об изменении данных
        /// </summary>
        event Action<IEnumerable<T>> DataChanged;

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="id">Id сущности</param>
        void Delete(int id);

        /// <summary>
        /// Добавление сущности
        /// </summary>
        /// <param name="entity">сущность</param>
        void Add(T entity);

        /// <summary>
        /// Вызов события об именении данных.
        /// </summary>
        void InvokeDataChanged();
    }
}
