using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ConsoleView
{
    class Program
    {
        static LibraryManager library = new LibraryManager();
        static void Main(string[] args)
        {
            
        }

        static void ShowReaders()
        {
            if (library.Readers.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Читателей нет");
                Console.ReadKey();
                return;
            }

            int index = 0;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("=== Список читателей ===\n");
                for (int i = 0; i < library.Readers.Count; i++)
                {
                    if (i == index) Console.ForegroundColor = ConsoleColor.Green;
                    var r = library.Readers[i];
                    Console.WriteLine($"{r.ID}. {r.Name,-15} {r.Address,-20} (Книг: {r.BooksBorrowed.Count})");
                    Console.ResetColor();
                }
                Console.WriteLine("\nСтрелки вверх/вниз — выбор, Enter — профиль, Esc — назад");

                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && index > 0) index--;
                if (key == ConsoleKey.DownArrow && index < library.Readers.Count - 1) index++;
                if (key == ConsoleKey.Enter) ShowReaderProfile(library.Readers[index]);
            } while (key != ConsoleKey.Escape);
        }

        static void ShowReaderProfile(Reader reader)
        {
            bool profileOpen = true;

            while (profileOpen)
            {
                Console.WriteLine($"=== Профиль читателя {reader.Name} ===");
                Console.WriteLine($"ID: {reader.ID}");
                Console.WriteLine($"Адрес: {reader.Address}");
                Console.WriteLine("Книги:");
                if (reader.BooksBorrowed.Count == 0)
                    Console.WriteLine("Книг нет");
                else
                {
                    foreach (Book book in reader.BooksBorrowed)
                    {
                        Console.WriteLine($"{book.Title} - {book.Author}");
                    }
                }

                Console.WriteLine("\n1. Изменить данные");
                Console.WriteLine("2. Удалить читателя");
                Console.WriteLine("3. Добавить книгу читателю");
                Console.WriteLine("4. Вернуть книгу");
                Console.WriteLine("0. Назад");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.Write("Новое имя (оставьте пустым, если без изменений): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            reader.Name = newName;
                        }

                        Console.Write("Новый адрес (оставьте пустым, если без изменений): ");
                        string newAddress = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newAddress))
                        {
                            reader.Address = newAddress;
                        }
                        break;
                    case ConsoleKey.D2:
                        library.DeleteReader(reader);
                        Console.Clear();
                        Console.WriteLine("Профиль удален. Нажмите любую клавишу, чтобы вернуться.");
                        Console.ReadKey();
                        profileOpen = false;
                        break;
                    case ConsoleKey.D3:
                        GiveBookToReader(reader);
                        break;
                    case ConsoleKey.D4:
                        ReturnBookFromReader(reader);
                        break;
                    case ConsoleKey.D0: profileOpen = false; break;
                }
            }
        }
        
        static void GiveBookToReader()
        {
            var available = library.Books.Where(b => b.IsAvailable).ToList();
            if (available.Count == 0)
            {
                Console.WriteLine("Нет доступных книг.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите книгу для выдачи:");
            for (int i = 0; i < available.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {available[i].Title} — {available[i].Author}");
            }
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= available.Count)
            {
                var book = available[choice - 1];
                reader.Books.Add(book);
                book.IsAvailable = false;
                Console.WriteLine("Книга выдана.");
            }
            Console.ReadKey();
        }

        static void ReturnBookFromReader()
        {

        }
    }
}
