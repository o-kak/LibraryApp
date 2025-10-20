using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;

namespace DataAccessLayer
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly AppDbContext _context;

        public EntityRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        /// <returns>Добавленная сущность (содержит установленный Id).</returns>
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Удаляет сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        public void Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Обновляет существующую сущность в базе данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными. EF Core будет отслеживать изменения.</param>
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Читает сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для чтения.</param>
        /// <returns>Найденная сущность или null, если сущность с указанным Id не найдена.</returns>
        public T ReadById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Читает все сущности типа T из базы данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей типа T.</returns>
        public IEnumerable<T> ReadAll()
        {
            return _context.Set<T>().ToList();
        }
    }
}
