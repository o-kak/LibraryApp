using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Model;
using Shared;

namespace Presenter
{
    internal class BookPresenter
    {
        private IModel<Book> BookLogic;
        private IView View;

        public BookPresenter(IModel<Book> bookLogic, IView view)
        {
            BookLogic = bookLogic;
            View = view;

            bookLogic.DataChanged += OnModelDataChanged;

            view.AddDataEvent += OnAddData;
            view.DeleteDataEvent += bookLogic.Delete;
        }

        private void OnModelDataChanged(IEnumerable<Book> books)
        {

        }

        private void OnAddData(EventArgs data)
        {
            BookEventArgs args = data as BookEventArgs;
            Book book = new Book();
            book.Id = args.Id;
            book.Title = args.Title;
            book.Author = args.Author;
            BookLogic.Add(book);
        }
    }
}
