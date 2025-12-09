using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ConsoleView
{
    public class LoanUIManager : ILoanView
    {
        public event Action<int, int> GiveBookEvent;
        public event Action<int, int> ReturnBookEvent;
        public event Action<int> GetReadersBorrowedBooksEvent;
        public event Action GetAvailableBooksEvent;

        private int _currentReaderId;

        public void StartLoanMenu(int id)
        {
            _currentReaderId = id;
            ConsoleKey key;
            bool active = true;

            while (active)
            {
                Console.Clear();
                Console.WriteLine($"--- Управление займами (Читатель ID: {_currentReaderId}) ---");
                Console.WriteLine("[1] Выдать книгу (использует данные из предыдущего экрана)");
                Console.WriteLine("[2] Вернуть книгу (использует данные из предыдущего экрана)");
                Console.WriteLine("[E] Сбросить и выйти в Главное меню");
                Console.Write("Выберите действие: ");

                key = Console.ReadKey().Key;
                Console.WriteLine(); // Новая строка после чтения

                switch (key)
                {
                    case ConsoleKey.D1:
                        GetAvailableBooksEvent?.Invoke();
                        break;

                    case ConsoleKey.D2:
                        GetReadersBorrowedBooksEvent(_currentReaderId);
                        break;

                    case ConsoleKey.E:
                        active = false; // Выход из цикла LoanMenu
                        break;
                    default:
                        ShowMessage("Неверный ввод.");
                        break;
                }
            }
        }

        public void ShowReadersBorrowedBooks(IEnumerable<EventArgs> books)
        {
            List<BookEventArgs> args = books as List<BookEventArgs>;
            if (args.Any())
            {
                for (int i = 0; i < args.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {args[i].Title} - {args[i].Author}");
                }
                PromptForBookSelection("Введите номер книги для возврата:", args.Count, (selectionIndex) =>
                {
                    int bookId = args[selectionIndex - 1].Id;
                    ReturnBookEvent?.Invoke(bookId, _currentReaderId);
                });
            }
            else
            {
                ShowMessage("У читателя нет книг для возврата.");
            }
        }

        public void ShowAvailableBooks(IEnumerable<EventArgs> books)
        {
            Console.WriteLine("Доступные книги:");
            List<BookEventArgs> bookArgs = (books as IEnumerable<BookEventArgs>).ToList();
            if (bookArgs.Any())
            {
                Console.WriteLine("\nДоступные книги:");
                for (int i = 0; i < bookArgs.Count; i++)
                    Console.WriteLine($"[{i + 1}] {bookArgs[i].Title} — {bookArgs[i].Author}");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= bookArgs.Count)
                {
                    try
                    {
                        GiveBookEvent?.Invoke(bookArgs[choice - 1].Id, _currentReaderId);
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
            else
            {
                Console.Write("нет");
                Console.ReadKey();
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }

        public void PromptForBookSelection(string prompt, int maxOptions, Action<int> selectionCallback)
        {
            Console.Write($"\n{prompt} ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice >= 1 && choice <= maxOptions)
                {
                    selectionCallback(choice);
                }
                else
                {
                    ShowMessage($"Неверный номер. Выберите число от 1 до {maxOptions}.");
                }
            }
            else
            {
                ShowMessage("Неверный формат ввода. Требуется число.");
            }
        }
    }
}
