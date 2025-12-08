using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleView
{
    public class MainMenu
    {
        private BooksUIManager BooksUIManager {  get; set; }
        private ReadersUIManager ReadersUIManager { get; set; }
        private LoanUIManager LoanUIManager { get; set; }

        public MainMenu(BooksUIManager booksUIManager, ReadersUIManager readersUIManager, LoanUIManager loanUIManager)
        {
            BooksUIManager = booksUIManager;
            ReadersUIManager = readersUIManager;
            LoanUIManager = loanUIManager;
        }

        public void ShowMainMenu()
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
                    case ConsoleKey.D1: ReadersUIManager.Start(); break;
                    case ConsoleKey.D2: BooksUIManager.Start(); ; break;
                    case ConsoleKey.D3: BooksUIManager.FilterBooksMenu(); break;
                    case ConsoleKey.D4: ReadersUIManager.AddReaderMenu(); break;
                }

            } while (!key.Equals(ConsoleKey.Escape));
        }
    }
}
