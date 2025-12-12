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
        private IBookService BookLogic;
        private IBookView View;

        public BookPresenter(IBookService bookLogic, IBookView view)
        {
            BookLogic = bookLogic;
            View = view;

            bookLogic.DataChanged += OnModelDataChanged;

            view.AddDataEvent += OnAddData;
            view.DeleteDataEvent += bookLogic.Delete;
            view.FilterDataByGenreEvent += OnFilterDataByGenre;
            view.FilterDataByAuthorEvent += OnFilterDataByAuthor;
            view.GetAvailableBooksEvent += OnGetAvailableBooks;
            view.GetBorrowedBooksEvent += OnGetBorrowedBooks;
            view.StartupEvent += Start;
        }

        /// <summary>
        /// Запуск вида
        /// </summary>
        private void Start()
        {
            BookLogic.InvokeDataChanged();
        }

        /// <summary>
        /// метод при изменении даных в модели
        /// </summary>
        /// <param name="books">список с данными</param>
        private void OnModelDataChanged(IEnumerable<Book> books)
        {
            List<BookEventArgs> args = new List<BookEventArgs>();
            foreach (Book book in books)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = book.ReaderId,
                });
            }
            View.Redraw(args);
        }

        /// <summary>
        /// добавление данных пользователем
        /// </summary>
        /// <param name="data">данные</param>
        private void OnAddData(EventArgs data)
        {
            BookEventArgs args = data as BookEventArgs;
            Book book = new Book();
            book.Id = args.Id;
            book.Title = args.Title;
            book.Author = args.Author;
            book.Genre = args.Genre;
            BookLogic.Add(book);
        }

        /// <summary>
        /// фильтрация по жанру
        /// </summary>
        /// <param name="genre">жанр</param>
        private void OnFilterDataByGenre(string genre)
        {
            List<BookEventArgs> args = new List<BookEventArgs>();
            IEnumerable<Book> sortedBooks = BookLogic.FilterByGenre(genre);
            foreach (Book book in sortedBooks)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = book.ReaderId,
                });
            }
            View.Redraw(args);
        }

        /// <summary>
        /// фильтрация по автору
        /// </summary>
        /// <param name="author">автор</param>
        private void OnFilterDataByAuthor(string author)
        {
            List<BookEventArgs> args = new List<BookEventArgs>();
            IEnumerable<Book> sortedBooks = BookLogic.FilterByAuthor(author);
            foreach (Book book in sortedBooks)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId= book.ReaderId,
                });
            }
            View.Redraw(args);
        }

        /// <summary>
        /// получить доступные книги
        /// </summary>
        private void OnGetAvailableBooks()
        {
            List<BookEventArgs> args = new List<BookEventArgs>();
            IEnumerable<Book> sortedBooks = BookLogic.GetAvailableBooks();
            foreach (Book book in sortedBooks)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = book.ReaderId,
                });
            }
            View.Redraw(args);
        }

        /// <summary>
        /// получить недоступные книги
        /// </summary>
        private void OnGetBorrowedBooks()
        {
            List<BookEventArgs> args = new List<BookEventArgs>();
            IEnumerable<Book> sortedBooks = BookLogic.GetBorrowedBooks();
            foreach (Book book in sortedBooks)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = book.ReaderId,
                });
            }
            View.Redraw(args);
        }
    }
}
