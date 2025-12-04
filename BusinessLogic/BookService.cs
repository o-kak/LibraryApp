using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataAccessLayer;

namespace BusinessLogic
{
    public class BookService : IModel<Book>
    {
        private IRepository<Book> BookRepository { get; set; }
        public event Action<IEnumerable<Book>> DataChanged;
        public BookService(IRepository<Book> bookRepository)
        {
            BookRepository = bookRepository;
        }

        /// <summary>
        /// добавить книгу
        /// </summary>
        /// <param name="title">название</param>
        /// <param name="author">автор</param>
        /// <param name="genre">жанр</param>
        public void Add(Book book)
        {
            BookRepository.Add(book);
            InvokeDataChanged();
            
        }

        /// <summary>
        /// удалить книгу
        /// </summary>
        /// <param name="bookId">id книги</param>
        public void Delete(int bookId)
        {
            BookRepository.Delete(bookId);
            InvokeDataChanged();
        }

        public void Update(Book book)
        {
            BookRepository.Update(book);
            InvokeDataChanged();
        }

        /// <summary>
        /// получить все книги
        /// </summary>
        /// <returns>список книг</returns>
        public IEnumerable<Book> GetAllBooks()
        {
            return BookRepository.ReadAll();
        }

        private void InvokeDataChanged()
        {
            DataChanged?.Invoke(new List<Book>(BookRepository.ReadAll()));
        }
    }
}
