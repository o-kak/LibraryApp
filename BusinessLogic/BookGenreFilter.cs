using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    public class BookGenreFilter : IFilter<Book>
    {
        private IRepository<Book> BookRepository {  get; set; }

        public BookGenreFilter(IRepository<Book> bookRepository)
        {
            BookRepository = bookRepository;
        }
        public IEnumerable<Book> Filter(string genre)
        {
            List<Book> allBooks = BookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Genre == genre);
        }
    }
}
