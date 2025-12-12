using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using BusinessLogic;
using Model;
using Shared;

namespace Presenter
{
    internal class ReaderPresenter
    {
        private IReaderService ReaderLogic;
        private ILoan LoanLogic;
        private IReaderView View;

        public ReaderPresenter(IReaderService readerLogic, IReaderView view, ILoan loanLogic)
        {
            this.ReaderLogic = readerLogic;
            this.View = view;
            this.LoanLogic = loanLogic;

            readerLogic.DataChanged += OnModelDataChanged;

            view.AddDataEvent += OnAddData;
            view.DeleteDataEvent += OnDeleteData;
            view.UpdateDataEvent += OnUpdateData;
            view.ReadByIdEvent += OnReadById;
            view.StartupEvent += Start;
            view.GetBorrowedBooksEvent += OnGetReadersBorrowedBooks;
        }

        /// <summary>
        /// хапуск вида
        /// </summary>
        private void Start()
        {
            ReaderLogic.InvokeDataChanged();
        }

        /// <summary>
        /// удаление данных
        /// </summary>
        /// <param name="id">Id сущности</param>
        private void OnDeleteData(int id)
        {
            List<Book> books = LoanLogic.GetReadersBorrowedBooks(id).ToList();
            if (books.Any())
            {
                foreach (Book book in books)
                {
                    LoanLogic.ReturnBook(book.Id, id);
                }
            }
            ReaderLogic.Delete(id);
        }

        /// <summary>
        /// на изменения данных в модели
        /// </summary>
        /// <param name="readers">список данных</param>
        private void OnModelDataChanged(IEnumerable<Reader> readers)
        {
            List<ReaderEventArgs> args = new List<ReaderEventArgs>();
            foreach (Reader reader in readers)
            {
                args.Add(new ReaderEventArgs()
                {
                    Id = reader.Id,
                    Name = reader.Name,
                    Address = reader.Address,
                });
            }
            View.Redraw(args);
        }

        /// <summary>
        /// добавление данных
        /// </summary>
        /// <param name="data">данные</param>
        private void OnAddData(EventArgs data)
        {
            ReaderEventArgs args = data as ReaderEventArgs;
            Reader reader = new Reader();
            reader.Id = args.Id;
            reader.Name = args.Name;
            reader.Address = args.Address;
            ReaderLogic.Add(reader);
        }

        /// <summary>
        /// обновление данных
        /// </summary>
        /// <param name="data">данные</param>
        private void OnUpdateData(EventArgs data)
        {
            ReaderEventArgs args = data as ReaderEventArgs;
            Reader reader = ReaderLogic.GetReader(args.Id);
            if (args.Name != null)
                reader.Name = args.Name;
            if (args.Address != null) 
                reader.Address = args.Address;
            ReaderLogic.Update(reader);
        }

        /// <summary>
        /// чтение пользователя и открытие его профиля
        /// </summary>
        /// <param name="id">Id читателя</param>
        private void OnReadById(int id)
        {
            Reader reader = ReaderLogic.GetReader(id);
            ReaderEventArgs args = new ReaderEventArgs()
            {
                Id = id,
                Name = reader.Name,
                Address = reader.Address
            };
            View.ShowReaderProfile(args);
        }

        /// <summary>
        /// получить книги на руках у читателя
        /// </summary>
        /// <param name="id">Id читателя</param>
        private void OnGetReadersBorrowedBooks(int id)
        {
            List<Book> books = LoanLogic.GetReadersBorrowedBooks(id).ToList();
            List<BookEventArgs> args = new List<BookEventArgs>();
            foreach (Book book in books)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = id

                });
            }
            View.ShowBorrowedBooks(args);
        }
    }
}
