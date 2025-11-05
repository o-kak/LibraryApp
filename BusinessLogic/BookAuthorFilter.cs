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
        private IRepository<Book> _bookRepository;

        public BookAuthorFilter(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> Filter(string author)
        {
            List<Book> allBooks = _bookRepository.ReadAll().ToList();
            return allBooks.Where(book => book.Author == author);
        }
    }
}
