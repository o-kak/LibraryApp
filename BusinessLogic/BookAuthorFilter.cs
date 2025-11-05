using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    internal class BookAuthorFilter : IFilter<Book>
    {
        private IRepository<Book> BookRepository {  get; set; }

        public BookAuthorFilter(IRepository<Book> bookRepository)
        {
            BookRepository = bookRepository;
        }

        public IEnumerable<Book> Filter(string author)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Author == author);
        }
    }
}
