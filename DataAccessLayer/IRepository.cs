using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        /// <returns>Добавленная сущность (содержит установленный Id).</returns>
        T Add(T entity);

        /// <summary>
        /// Удаляет сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        void Delete(int id);

        /// <summary>
        /// Обновляет существующую сущность в базе данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными. EF Core будет отслеживать изменения.</param>
        void Update(T entity);

        /// <summary>
        /// Читает сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для чтения.</param>
        /// <returns>Найденная сущность или null, если сущность с указанным Id не найдена.</returns>
        T ReadById(int id);

        /// <summary>
        /// Читает все сущности типа T из базы данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей типа T.</returns>
        IEnumerable<T> ReadAll();
    }

}
