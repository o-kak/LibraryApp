using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Model;
using Shared;

namespace ConsoleView
{
    public class LoanUIManager : ILoanView
    {
        public event Action<int, int> GiveBookEvent;
        public event Action<int, int> ReturnBookEvent;
        public event Action<int> GetReadersBorrowedBooksEvent;
        IBookService BookService { get; set; }

        public LoanUIManager(IBookService bookService)
        {
            BookService = bookService;
        }

        /// <summary>
        /// выдача книги
        /// </summary>
        /// <param name="reader">читатель</param>
        public void GiveBookToReader(EventArgs data)
        {
            ReaderEventArgs reader = data as ReaderEventArgs;
            var books = BookService.GetAvailableBooks().ToList();
            //if (!books.Any())
            //{
            //    Console.WriteLine("\nНет доступных книг.");
            //    Console.ReadKey();
            //    return;
            //}

            Console.WriteLine("\nДоступные книги:");
            for (int i = 0; i < books.Count; i++)
                Console.WriteLine($"[{i + 1}] {books[i].Title} — {books[i].Author}");

            Console.Write("\nВведите номер книги: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= books.Count)
            {
                try
                {
                    GiveBookEvent?.Invoke(books[choice - 1].Id, reader.Id);
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

        /// <summary>
        /// возврат книги
        /// </summary>
        /// <param name="reader">читатаель</param>
        public void ReturnBookFromReader(EventArgs data)
        {
            ReaderEventArgs reader = data as ReaderEventArgs;
            //if (!LoanService.GetReadersBorrowedBooks(reader.Id).Any())
            //{
            //    Console.WriteLine("\nУ читателя нет книг.");
            //    Console.ReadKey();
            //    return;
            //}

            Console.WriteLine("\nКниги у читателя:");
            GetReadersBorrowedBooksEvent.Invoke(reader.Id);

            Console.Write("\nВведите номер книги: ");
            //if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= LoanService.GetReadersBorrowedBooks(reader.Id).ToList().Count)
            //{
            //    try
            //    {
            //        LoanService.ReturnBook(LoanService.GetReadersBorrowedBooks(reader.Id).ToList()[choice - 1].Id, reader.Id);
            //        Console.WriteLine("\nКнига возвращена!");
            //    }
            //    catch (InvalidOperationException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("\nНеверный выбор.");
            //}
            //Console.ReadKey();
        }

        public void ShowReadersBorrowedBooks(IEnumerable<EventArgs> books)
        {
            IEnumerable<BookEventArgs> bookArgs = books as IEnumerable<BookEventArgs>;
            if (bookArgs.Any())
            {
                Console.Write(string.Join(", ", bookArgs.Select(b => b.Title)));
            }
            else Console.Write("нет");
        }
    }
}
