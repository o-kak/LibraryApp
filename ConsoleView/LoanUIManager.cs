using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Model;

namespace ConsoleView
{
    internal class LoanUIManager
    {
        private LoanService LoanService { get; set; }

        public LoanUIManager(LoanService loanService)
        {
            LoanService = loanService;
        }

        public void GiveBookToReader(Reader reader)
        {
            var books = LoanService.GetAvailableBooks().ToList();
            if (!books.Any())
            {
                Console.WriteLine("\nНет доступных книг.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nДоступные книги:");
            for (int i = 0; i < books.Count; i++)
                Console.WriteLine($"[{i + 1}] {books[i].Title} — {books[i].Author}");

            Console.Write("\nВведите номер книги: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= books.Count)
            {
                try
                {
                    LoanService.GiveBook(books[choice - 1].Id, reader.Id);
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

        public void ReturnBookFromReader(Reader reader)
        {
            if (!LoanService.GetReadersBorrowedBooks(reader.Id).Any())
            {
                Console.WriteLine("\nУ читателя нет книг.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nКниги у читателя:");
            for (int i = 0; i < LoanService.GetReadersBorrowedBooks(reader.Id).ToList().Count; i++)
                Console.WriteLine($"[{i + 1}] {LoanService.GetReadersBorrowedBooks(reader.Id).ToList()[i].Title}");

            Console.Write("\nВведите номер книги: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= LoanService.GetReadersBorrowedBooks(reader.Id).ToList().Count)
            {
                try
                {
                    LoanService.ReturnBook(LoanService.GetReadersBorrowedBooks(reader.Id).ToList()[choice - 1].Id, reader.Id);
                    Console.WriteLine("\nКнига возвращена!");
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
