using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Shared;
using Model;

namespace Presenter
{
    public class LoanPresenter
    {
        private ILoan LoanLogic;
        private ILoanView LoanView;
        private IBookService BookLogic;

        public LoanPresenter(ILoan loanLogic, ILoanView loanView, IBookService bookLogic)
        {
            LoanLogic = loanLogic;
            LoanView = loanView;

            loanView.ReturnBookEvent += loanLogic.ReturnBook;
            loanView.GiveBookEvent += loanLogic.GiveBook;
            loanView.GetReadersBorrowedBooksEvent += OnGetReadersBorrowedBooks;
            loanView.GetAvailableBooksEvent += OnGetAvailableBooks;
            BookLogic = bookLogic;
        }

        public void OnGetReadersBorrowedBooks(int id)
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
                });
                LoanView.ShowReadersBorrowedBooks(args);
            }
        }

        public void OnGetAvailableBooks()
        {
            List<Book> books = BookLogic.GetAvailableBooks().ToList();
            List<BookEventArgs> args = new List<BookEventArgs>();
            foreach(Book book in books)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                });
            }
            LoanView.ShowAvailableBooks(args);
        }

        public void GiveBook(int readerId, int bookId)
        {
            LoanLogic.GiveBook(readerId, bookId);
            LoanView.ShowMessage("Книга выдана!");
        }

        public void ReturnBook(int readerId, int bookId)
        {
            LoanLogic.ReturnBook(readerId, bookId);
            LoanView.ShowMessage("Книга возвращена");
        }
    }
}