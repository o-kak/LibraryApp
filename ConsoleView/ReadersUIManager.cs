using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ConsoleView
{
    public class ReadersUIManager : IReaderView
    {
        public event Action<EventArgs> AddDataEvent;
        public event Action<int> DeleteDataEvent;
        public event Action<EventArgs> UpdateDataEvent;
        public event Action<int> ReadByIdEvent;
        public event Action GetAvailableBooksEvent;
        public event Action<int> GetBorrowedBooksEvent;
        public event Action StartupEvent;

        private ILoanView LoanUIManager { get; set; }

        public void Start()
        {
            StartupEvent?.Invoke();
        }
        public ReadersUIManager(ILoanView loanUIManager)
        {
            LoanUIManager = loanUIManager;
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
                Console.WriteLine($"Книги на руках: ");
                GetBorrowedBooksEvent?.Invoke(reader.Id);
                Console.WriteLine("\n[1] Изменить данные");
                Console.WriteLine("[2] Удалить читателя");
                Console.WriteLine("[3] Взять/вернуть книгу");
                Console.WriteLine("[Esc] Назад в список");

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        ReaderEventArgs args = new ReaderEventArgs();
                        args.Id = reader.Id;
                        Console.Write("\nНовое имя (пусто — без изменений): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                            args.Name = newName;

                        Console.Write("Новый адрес (пусто — без изменений): ");
                        string newAddress = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newAddress))
                            args.Address = newAddress;
                        UpdateDataEvent?.Invoke(args);
                        Console.ReadKey();
                        return;
                    case ConsoleKey.D2:
                        Console.WriteLine("\nЧитатель удалён.");
                        DeleteDataEvent?.Invoke(reader.Id);
                        Console.ReadKey();
                        return;

                    case ConsoleKey.D3:
                        LoanUIManager.StartLoanMenu(reader.Id);
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

        public void ShowBorrowedBooks(IEnumerable<EventArgs> args)
        {
            List<BookEventArgs> books = args as List<BookEventArgs>;
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {books[i].Title} — {books[i].Author}");
            }
        }
    }
}
