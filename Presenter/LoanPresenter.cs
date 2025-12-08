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
        private IReaderView ReaderView;
        private IBookService BookLogic;

        public LoanPresenter(ILoan loanLogic, ILoanView loanView, IReaderView readerView, IBookService bookLogic)
        {
            LoanLogic = loanLogic;
            LoanView = loanView;

            loanView.ReturnBookEvent += loanLogic.ReturnBook;
            loanView.GiveBookEvent += loanLogic.GiveBook;
            loanView.GetReadersBorrowedBooksEvent += OnGetReadersBorrowedBooks;
            readerView.GetReadersBorrowedBooksEvent += OnGetReadersBorrowedBooks;
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

        public void GiveBookToReader(EventArgs data)
        {
            ReaderEventArgs reader = data as ReaderEventArgs;
            var books = BookLogic.GetAvailableBooks().ToList();
            if (!books.Any())
            {
                LoanView.ShowMessage("\nНет доступных книг.");
                return;
            }

            List<BookEventArgs> args = new List<BookEventArgs>();
            foreach (Book book in books)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                });
            }
            LoanView.ShowAvailableBooks(args);

            Console.Write("\nВведите номер книги: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= books.Count)
            {
                try
                {
                    LoanLogic.GiveBook(books[choice - 1].Id, reader.Id);
                    Console.WriteLine("\nКнига выдана!");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("\nНеверный выбор.");
            }
            Console.ReadKey();
        }
    }
}