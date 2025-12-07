using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Model;
using Shared;

namespace ConsoleView
{
    internal class ReadersUIManager : IReaderView
    {
        public event Action<EventArgs> AddDataEvent;
        public event Action<int> DeleteDataEvent;
        public event Action<EventArgs> UpdateDataEvent;
        public event Action<int> ReadByIdEvent;
        private IReaderService ReaderService { get; set; }
        private LoanUIManager LoanUIManager { get; set; }
        private ILoan LoanService { get; set; }

        public ReadersUIManager(IReaderService readerService, LoanUIManager loanUIManager, ILoan loanService)
        {
            ReaderService = readerService;
            LoanUIManager = loanUIManager;
            LoanService = loanService;
        }

        /// <summary>
        /// список всех читателей
        /// </summary>
        public void Redraw(IEnumerable<EventArgs> data)
        {
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== СПИСОК ЧИТАТЕЛЕЙ ===\n");

                List<ReaderEventArgs> readers = (data as IEnumerable<ReaderEventArgs>).ToList();

                if (!readers.Any())
                {
                    Console.WriteLine("Нет зарегистрированных читателей.");
                    Console.WriteLine("\nНажмите любую клавишу для выхода...");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < readers.Count; i++)
                {
                    if (i == index)
                        Console.BackgroundColor = ConsoleColor.DarkGray;

                    Console.WriteLine($"Id: {readers[i].Id} Имя: {readers[i].Name} ({readers[i].Address})");

                    Console.ResetColor();
                }

                Console.WriteLine("\nСтрелки ↑↓ — выбор, Enter — открыть профиль, Esc — выход");

                key = Console.ReadKey().Key;

                if (key == ConsoleKey.UpArrow)
                    index = (index <= 0) ? readers.Count - 1 : index - 1;
                else if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % readers.Count;
                else if (key == ConsoleKey.Enter)
                    ReadByIdEvent?.Invoke(readers[index].Id);

            } while (key != ConsoleKey.Escape);
        }

        /// <summary>
        /// профиль читателя
        /// </summary>
        /// <param name="readerId">id читателя</param>
        public void ShowReaderProfile(ReaderEventArgs reader)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine($"=== ПРОФИЛЬ ЧИТАТЕЛЯ ===\n");
                Console.WriteLine($"ID: {reader.Id}");
                Console.WriteLine($"Имя: {reader.Name}");
                Console.WriteLine($"Адрес: {reader.Address}");
                Console.WriteLine($"Книги на руках: {(LoanService.GetReadersBorrowedBooks(reader.Id).Any() ? string.Join(", ", LoanService.GetReadersBorrowedBooks(reader.Id).Select(b => b.Title)) : "нет")}");
                Console.WriteLine("\n[1] Изменить данные");
                Console.WriteLine("[2] Удалить читателя");
                Console.WriteLine("[3] Взять книгу");
                Console.WriteLine("[4] Вернуть книгу");
                Console.WriteLine("[Esc] Назад в список");

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        Console.Write("\nНовое имя (пусто — без изменений): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                            reader.Name = newName;

                        Console.Write("Новый адрес (пусто — без изменений): ");
                        string newAddress = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newAddress))
                            reader.Address = newAddress;
                        break;

                    case ConsoleKey.D2:
                        Console.WriteLine("\nЧитатель удалён.");
                        DeleteDataEvent?.Invoke(reader.Id);
                        Console.ReadKey();
                        return;

                    case ConsoleKey.D3:
                        LoanUIManager.GiveBookToReader(reader);
                        break;

                    case ConsoleKey.D4:
                        LoanUIManager.ReturnBookFromReader(reader);
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        /// <summary>
        /// меню добавления читателя
        /// </summary>
        public void AddReaderMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ ЧИТАТЕЛЯ ===\n");

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Поле должно быть заполнено");
                name = Console.ReadLine();
            }

            Console.Write("Введите адрес: ");
            string address = Console.ReadLine();
            while (string.IsNullOrEmpty(address))
            {
                Console.WriteLine("Поле должно быть заполнено");
                address = Console.ReadLine();
            }

            Console.WriteLine("\nЧитатель добавлен!");
            AddDataEvent?.Invoke(new ReaderEventArgs()
            {
                Name = name,
                Address = address
            });

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
