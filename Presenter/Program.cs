using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Ninject;
using ConsoleView;

namespace Presenter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== Главное меню выбора UI =====");
                Console.WriteLine("1. Запустить Консольный режим (Console UI)");
                Console.WriteLine("2. Запустить WinForms режим (WinForms UI)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите вариант (0, 1 или 2): ");

                string choice = Console.ReadLine();;

                switch (choice)
                {
                    case "1":

                        IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
                        BookService bookService = ninjectKernel.Get<BookService>();
                        ReaderService readerService = ninjectKernel.Get<ReaderService>();
                        LoanService loanService = ninjectKernel.Get<LoanService>();

                        BooksUIManager booksUIManager = new BooksUIManager();
                        LoanUIManager loanUIManager = new LoanUIManager();
                        ReadersUIManager readersUIManager = new ReadersUIManager(loanUIManager);

                        ReaderPresenter readerPresenter = new ReaderPresenter(readerService, readersUIManager);
                        BookPresenter bookPresenter = new BookPresenter(bookService, booksUIManager);
                        LoanPresenter loanPresenter = new LoanPresenter(loanService, loanUIManager, readersUIManager, bookService);
                        MainMenu mainMenu = new MainMenu(booksUIManager, readersUIManager, loanUIManager);
                        mainMenu.ShowMainMenu();
                        break;

                    case "2":
                        Console.WriteLine("пипипу");
                        Console.ReadKey();
                        break;

                    case "0":
                        running = false;
                        continue;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        Console.ReadKey();
                        continue;
                }
            }
        }
    }
}
