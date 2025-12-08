using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using BusinessLogic;
using Model;
using Ninject;

namespace ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            //LibraryManager manager = new LibraryManager();
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            BooksUIManager booksManager = ninjectKernel.Get<BooksUIManager>();
            ReadersUIManager readersManager = ninjectKernel.Get<ReadersUIManager>();
            LoanUIManager loanManager = ninjectKernel.Get<LoanUIManager>();

            ShowMainMenu(booksManager, readersManager, loanManager);
        }

        /// <summary>
        /// главное меню
        /// </summary>
        /// <param name="b">ui для книг</param>
        /// <param name="r">ui для читателей</param>
        /// <param name="l">ui для выдачи и возвратов</param>
        static void ShowMainMenu(BooksUIManager b, ReadersUIManager r, LoanUIManager l)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ БИБИЛИОТЕКОЙ ===");
                Console.WriteLine("[1] Список читателей");
                Console.WriteLine("[2] Список книг");
                Console.WriteLine("[3] Поиск книг по жанру/автору");
                Console.WriteLine("[4] Добавить читателя");
                Console.WriteLine("[Esc] Выход");

                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1: r.ShowReaders(); break;
                    case ConsoleKey.D2: Console.WriteLine("Placeholder"); ; break;
                    case ConsoleKey.D3: b.FilterBooksMenu(); break;
                    case ConsoleKey.D4: r.AddReaderMenu(); break;
                }

            } while (!key.Equals(ConsoleKey.Escape));
        }
    }
}
