using Dapper;
using Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public DapperRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        /// <summary>
        /// Создает и возвращает новое соединение с базой данных PostgreSQL.
        /// </summary>
        /// <returns>Открытое соединение с базой данных.</returns>
        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// Автоматически определяет столбцы для вставки на основе свойств объекта,
        /// и получает сгенерированный Id из PostgreSQL с помощью RETURNING.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        /// <returns>Добавленная сущность с установленным Id.</returns>
        public T Add(T entity)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string tableNameWithSchema;
                tableNameWithSchema = $"\"{_tableName}\"";
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanWrite && p.Name != nameof(IDomainObject.Id))
                    .ToList();

                var columnNames = properties.Select(p => $"\"{p.Name}\"").ToList();
                var parameterNames = properties.Select(p => $"@{p.Name}").ToList();

                string insertQuery = $"INSERT INTO {tableNameWithSchema} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", parameterNames)}) RETURNING \"Id\";";

                int newId = dbConnection.QuerySingle<int>(insertQuery, entity);

                var idProperty = typeof(T).GetProperty(nameof(IDomainObject.Id));
                if (idProperty != null && idProperty.CanWrite)
                {
                    idProperty.SetValue(entity, newId);
                }

                return entity;
            }
        }

        /// <summary>
        /// Удаляет сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string deleteQuery = $"DELETE FROM \"{_tableName}\" WHERE \"Id\" = @Id";
                dbConnection.Execute(deleteQuery, new { Id = id });
            }
        }

        /// <summary>
        /// Обновляет существующую сущность в базе данных.
        /// Автоматически определяет столбцы для обновления на основе свойств объекта (кроме Id).
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными. Должна содержать корректный Id.</param>
        public void Update(T entity)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string tableNameWithSchema;
                tableNameWithSchema = $"\"{_tableName}\"";

                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanWrite && p.Name != nameof(IDomainObject.Id))
                    .ToList();

                var setClauses = properties.Select(p => $"\"{p.Name}\" = @{p.Name}").ToList();

                string updateQuery = $"UPDATE {tableNameWithSchema} SET {string.Join(", ", setClauses)} WHERE \"Id\" = @Id";

                dbConnection.Execute(updateQuery, entity);
            }
        }

        /// <summary>
        /// Читает сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для чтения.</param>
        /// <returns>Найденная сущность или null, если сущность с указанным Id не найдена.</returns>
        public T ReadById(int id)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string selectQuery = $"SELECT * FROM \"{_tableName}\" WHERE \"Id\" = @Id";
                return dbConnection.QueryFirstOrDefault<T>(selectQuery, new { Id = id });
            }
        }

        /// <summary>
        /// Читает все сущности типа T из базы данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей типа T.</returns>
        public IEnumerable<T> ReadAll()
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string selectQuery = $"SELECT * FROM \"{_tableName}\"";
                return dbConnection.Query<T>(selectQuery);
            }
        }
    }
}
