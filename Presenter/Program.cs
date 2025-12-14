using BusinessLogic;
using ConsoleView;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsView;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Presenter
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== Главное меню выбора UI =====");
                Console.WriteLine("1. Запустить Консольный режим (Console UI)");
                Console.WriteLine("2. Запустить WinForms режим (WinForms UI)");
                Console.WriteLine("3. Запустить WPF режим (WPF UI)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите вариант (0, 1, 2 или 3): ");

                string choice = Console.ReadLine();;

                switch (choice)
                {
                    case "1":

                        RunConsoleMode();
                        break;

                    case "2":
                        RunWinFormsMode();
                        break;

                    case "3":
                        RunWPFMode();
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

        static void RunConsoleMode() 
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            BookService bookService = ninjectKernel.Get<BookService>();
            ReaderService readerService = ninjectKernel.Get<ReaderService>();
            LoanService loanService = ninjectKernel.Get<LoanService>();

            BooksUIManager booksUIManager = new BooksUIManager();
            LoanUIManager loanUIManager = new LoanUIManager();
            ReadersUIManager readersUIManager = new ReadersUIManager(loanUIManager);

            ReaderPresenter readerPresenter = new ReaderPresenter(readerService, readersUIManager, loanService);
            BookPresenter bookPresenter = new BookPresenter(bookService, booksUIManager);
            LoanPresenter loanPresenter = new LoanPresenter(loanService, loanUIManager, bookService);
            ConsoleView.MainMenu mainMenu = new ConsoleView.MainMenu(booksUIManager, readersUIManager, loanUIManager);
            mainMenu.ShowMainMenu();
        }

        [STAThread]
        static void RunWinFormsMode() 
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            BookService winFormsBookService = ninjectKernel.Get<BookService>();
            ReaderService winFormsReaderService = ninjectKernel.Get<ReaderService>();
            LoanService winFormsLoanService = ninjectKernel.Get<LoanService>();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var form1 = new Form1();

            var bookView = form1._bookView; 
            var readerView = form1._readerView; 
            var loanView = form1._loanView; 

            var bookPresenter = new BookPresenter(winFormsBookService, bookView);
            var readerPresenter = new ReaderPresenter(winFormsReaderService, readerView, winFormsLoanService);
            var loanPresenter = new LoanPresenter(winFormsLoanService, loanView, winFormsBookService);

            System.Windows.Forms.Application.Run(form1);    
        }

        static void RunWPFMode() 
        {

        }
    }
}
