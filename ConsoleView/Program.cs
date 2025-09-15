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
        static void Main(string[] args)
        {
            LibraryManager manager = new LibraryManager();

            manager.AddReader("Иван Иванов", "ул. Пушкина, д. 10");
            manager.AddReader("Анна Смирнова", "ул. Ленина, д. 5");

            manager.AddBook("Война и мир", "Толстой", "Роман");
            manager.AddBook("Преступление и наказание", "Достоевский", "Роман");
            manager.AddBook("Мастер и Маргарита", "Булгаков", "Фантастика");

            ShowMainMenu(manager);
        }

        static void ShowMainMenu(LibraryManager manager)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ БИБИЛИОТЕКОЙ ===");
                Console.WriteLine("[1] Список читателй");
                Console.WriteLine("[2] Список книг");
                Console.WriteLine("[3] Поиск книг по жанру/автору");
                Console.WriteLine("[Esc] Выход");

                key = Console.ReadKey().Key;
                switch(key)
                {
                    case ConsoleKey.D1: ShowReaders(manager); break;
                    case ConsoleKey.D2: ShowBooksMenu(manager); break;
                    case ConsoleKey.D3: FilterBooksMenu(manager); break;
                }
                    
            } while (!key.Equals(ConsoleKey.Escape));
        }
        static void ShowReaders(LibraryManager manager)
        {
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== СПИСОК ЧИТАТЕЛЕЙ ===\n");

                var readers = manager.Readers.ToList();

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

                    Console.WriteLine($"{readers[i].ID}. {readers[i].Name} ({readers[i].Address})");

                    Console.ResetColor();
                }

                Console.WriteLine("\nСтрелки ↑↓ — выбор, Enter — открыть профиль, Esc — выход");

                key = Console.ReadKey().Key;

                if (key == ConsoleKey.UpArrow)
                    index = (index <= 0) ? readers.Count - 1 : index - 1;
                else if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % readers.Count;
                else if (key == ConsoleKey.Enter)
                    ShowReaderProfile(manager, readers[index]);

            } while (key != ConsoleKey.Escape);
        }

        static void ShowReaderProfile(LibraryManager manager, Reader reader)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine($"=== ПРОФИЛЬ ЧИТАТЕЛЯ ===\n");
                Console.WriteLine($"ID: {reader.ID}");
                Console.WriteLine($"Имя: {reader.Name}");
                Console.WriteLine($"Адрес: {reader.Address}");
                Console.WriteLine($"Книги на руках: {(reader.BooksBorrowed.Any() ? string.Join(", ", reader.BooksBorrowed.Select(b => b.Title)) : "нет")}");
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
                        manager.DeleteReader(reader);
                        Console.WriteLine("\nЧитатель удалён.");
                        Console.ReadKey();
                        return;

                    case ConsoleKey.D3:
                        GiveBookToReader(manager, reader);
                        break;

                    case ConsoleKey.D4:
                        ReturnBookFromReader(manager, reader);
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        static void GiveBookToReader(LibraryManager manager, Reader reader)
        {
            var books = manager.GetAvailableBooks().ToList();
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
                    manager.GiveBook(books[choice - 1], reader);
                    Console.WriteLine("\nКнига выдана!");
                }
                catch(InvalidOperationException ex)
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

        static void ReturnBookFromReader(LibraryManager manager, Reader reader)
        {
            if (!reader.BooksBorrowed.Any())
            {
                Console.WriteLine("\nУ читателя нет книг.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nКниги у читателя:");
            for (int i = 0; i < reader.BooksBorrowed.Count; i++)
                Console.WriteLine($"[{i + 1}] {reader.BooksBorrowed[i].Title}");

            Console.Write("\nВведите номер книги: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= reader.BooksBorrowed.Count)
            {
                try
                {
                    manager.ReturnBook(reader.BooksBorrowed[choice - 1], reader);
                    Console.WriteLine("\nКнига возвращена!");
                }
                catch(InvalidOperationException ex)
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

        static void ShowBooks(IEnumerable<Book> books)
        {
            Console.Clear();
            Console.WriteLine("=== СПИСОК КНИГ ===\n");

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} — {book.Author} [{book.Genre}] ");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void FilterBooksMenu(LibraryManager manager)
        {
            Console.Clear();
            Console.WriteLine("=== ФИЛЬТР КНИГ ===\n");
            Console.WriteLine("[1] По жанру");
            Console.WriteLine("[2] По автору");

            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.D1)
            {
                Console.Write("\nВведите жанр: ");
                string genre = Console.ReadLine();
                while (string.IsNullOrEmpty(genre))
                {
                    Console.WriteLine("Введенное значение не должно быть пустым");
                }
                ShowBooks(manager.FilterBooksByGenre(genre ?? ""));
            }
            else if (key == ConsoleKey.D2)
            {
                Console.Write("\nВведите автора: ");
                string author = Console.ReadLine();
                while (string.IsNullOrEmpty(author))
                {
                    Console.WriteLine("Введенное значение не должно быть пустым");
                }
                ShowBooks(manager.FilterBooksByAuthor(author ?? ""));
            }
        }

        static void ShowBooksMenu(LibraryManager manager)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("=== КНИГИ ===\n");
                Console.WriteLine("[1] Показать все книги");
                Console.WriteLine("[2] Показать доступные книги");
                Console.WriteLine("[3] Показать выданные книги");
                Console.WriteLine("[Esc] Назад");

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        ShowBooks(manager.Books);
                        break;
                    case ConsoleKey.D2:
                        ShowBooks(manager.GetAvailableBooks());
                        break;
                    case ConsoleKey.D3:
                        ShowBooks(manager.GetBorrowedBooks());
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }
    }
}
