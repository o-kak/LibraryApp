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
                string insertQuery = "";
                object parameters = null;
                if (typeof(T) == typeof(Book))
                {
                    // Приводим entity к типу Book
                    var book = entity as Book;
                    if (book == null)
                    {
                        throw new ArgumentException("entity is not of type Book");
                    }
                    insertQuery = $"INSERT INTO \"{_tableName}\" (\"Title\", \"Author\", \"Genre\", \"IsAvailable\", \"ReaderId\") VALUES (@Title, @Author, @Genre, @IsAvailable, @ReaderId) RETURNING \"Id\";";
                    parameters = new
                    {
                        Title = book.Title,
                        Author = book.Author,
                        Genre = book.Genre,
                        IsAvailable = book.IsAvailable,
                        ReaderId = book.ReaderId
                    };
                    book.Id = dbConnection.QuerySingle<int>(insertQuery, parameters); // Устанавливаем Id
                    return entity;

                }
                else if (typeof(T) == typeof(Reader))
                {
                    var reader = entity as Reader;
                    if (reader == null)
                    {
                        throw new ArgumentException("entity is not of type Reader");
                    }
                    insertQuery = $"INSERT INTO \"{_tableName}\" (\"Name\", \"Address\") VALUES (@Name, @Address) RETURNING \"Id\";";
                    parameters = new
                    {
                        Name = reader.Name,
                        Address = reader.Address
                    };
                    reader.Id = dbConnection.QuerySingle<int>(insertQuery, parameters); // Устанавливаем Id
                    return entity;
                }
                else
                {
                    throw new ArgumentException($"Тип {typeof(T).Name} не поддерживается.");
                }
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
                string updateQuery = "";
                object parameters = null;

                if (typeof(T) == typeof(Book))
                {
                    // Приводим entity к типу Book
                    var book = entity as Book;
                    if (book == null)
                    {
                        throw new ArgumentException("entity is not of type Book");
                    }

                    updateQuery = $"UPDATE \"{_tableName}\" SET \"Title\" = @Title, \"Author\" = @Author, \"Genre\" = @Genre, \"IsAvailable\" = @IsAvailable, \"ReaderId\" = @ReaderId WHERE \"Id\" = @Id";

                    parameters = new
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        Genre = book.Genre,
                        IsAvailable = book.IsAvailable,
                        ReaderId = book.ReaderId
                    };
                    dbConnection.Execute(updateQuery, parameters);
                    return;

                }
                else if (typeof(T) == typeof(Reader))
                {
                    var reader = entity as Reader;
                    if (reader == null)
                    {
                        throw new ArgumentException("entity is not of type Reader");
                    }
                    updateQuery = $"UPDATE \"{_tableName}\" SET \"Name\" = @Name, \"Address\" = @Address WHERE \"Id\" = @Id";
                    parameters = new
                    {
                        Id = reader.Id,
                        Name = reader.Name,
                        Address = reader.Address
                    };
                    dbConnection.Execute(updateQuery, parameters);
                    return;
                }
                else
                {
                    throw new ArgumentException($"Тип {typeof(T).Name} не поддерживается.");
                }
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
