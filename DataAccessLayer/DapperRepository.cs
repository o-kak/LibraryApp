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
                string insertQuery = $"INSERT INTO \"{_tableName}\" ({GetColumnNamesWithoutId()}) VALUES ({GetParameters()}) RETURNING Id;";

                var parameters = new DynamicParameters(entity);

                int insertedId = dbConnection.QuerySingle<int>(insertQuery, parameters);

                typeof(T).GetProperty("Id")?.SetValue(entity, insertedId);

                return entity;
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string deleteQuery = $"DELETE FROM \"{_tableName}\" WHERE Id = @Id";
                dbConnection.Execute(deleteQuery, new { Id = id });
            }
        }

        public void Update(T entity)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string updateQuery = $"UPDATE \"{_tableName}\" SET {GetColumnUpdates()} WHERE Id = @Id";
                var parameters = new DynamicParameters(entity);
                dbConnection.Execute(updateQuery, parameters);
            }
        }

        public T ReadById(int id)
        {
            using (IDbConnection dbConnection = CreateConnection())
            {
                string selectQuery = $"SELECT * FROM \"{_tableName}\"";
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


        // Helper Methods

        private string GetColumnNamesWithoutId()
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" && !p.PropertyType.IsConstructedGenericType && !p.PropertyType.IsClass).Select(p => p.Name);
            return string.Join(", ", properties);
        }

        private string GetParameters()
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" && !p.PropertyType.IsConstructedGenericType && !p.PropertyType.IsClass).Select(p => "@" + p.Name);
            return string.Join(", ", properties);
        }

        private string GetColumnUpdates()
        {
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id" && !p.PropertyType.IsConstructedGenericType && !p.PropertyType.IsClass).Select(p => $"{p.Name} = @{p.Name}");
            return string.Join(", ", properties);
        }
    }
}
