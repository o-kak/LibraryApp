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

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

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
        

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string deleteQuery = $"DELETE FROM \"{_tableName}\" WHERE \"Id\" = @Id";
                dbConnection.Execute(deleteQuery, new { Id = id });
            }
        }

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

        public T ReadById(int id)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string selectQuery = $"SELECT * FROM \"{_tableName}\" WHERE \"Id\" = @Id";
                return dbConnection.QueryFirstOrDefault<T>(selectQuery, new { Id = id });
            }
        }

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
