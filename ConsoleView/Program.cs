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

        /// <summary>
        /// главное меню
        /// </summary>
        /// <param name="manager">менеджер бибилиотеки</param>
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
                Console.WriteLine("[4] Добавить читателя");
                Console.WriteLine("[Esc] Выход");

                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1: ShowReaders(manager); break;
                    case ConsoleKey.D2: ShowBooksMenu(manager); break;
                    case ConsoleKey.D3: FilterBooksMenu(manager); break;
                    case ConsoleKey.D4: AddReaderMenu(manager); break;
                }

            } while (!key.Equals(ConsoleKey.Escape));
        }

        /// <summary>
        /// показать список читателей
        /// </summary>
        /// <param name="manager">менеджер бибилиотеки</param>
        static void ShowReaders(LibraryManager manager)
        {
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== СПИСОК ЧИТАТЕЛЕЙ ===\n");

                List<Reader> readers = manager.Readers.ToList();

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

        /// <summary>
        /// профиль читатателя и работа с читатаелем
        /// </summary>
        /// <param name="manager">менеджер библиотеки</param>
        /// <param name="reader">читатель</param>
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

        /// <summary>
        /// выдать книгу
        /// </summary>
        /// <param name="manager">менеджер</param>
        /// <param name="reader">читатель</param>
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

        /// <summary>
        /// вернуть книгу
        /// </summary>
        /// <param name="manager">менеджер</param>
        /// <param name="reader">читатель</param>
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

        /// <summary>
        /// показать список книг
        /// </summary>
        /// <param name="books">список книг</param>
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

        /// <summary>
        /// фильтрация книг по жанру и автору
        /// </summary>
        /// <param name="manager">менеджер</param>
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

        /// <summary>
        /// меню работы с книгами
        /// </summary>
        /// <param name="manager">менеджер</param>
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
                Console.WriteLine("[4] Добавить книгу");
                Console.WriteLine("[5] Удалить книгу");
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
                    case ConsoleKey.D4:
                        AddBookMenu(manager);
                        break;
                    case ConsoleKey.D5:
                        DeleteBookMenu(manager);
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        /// <summary>
        /// добавить новую книгу
        /// </summary>
        /// <param name="manager">менеджер</param>
        static void AddBookMenu(LibraryManager manager)
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ КНИГИ ===\n");

            Console.Write("Введите название: ");
            string title = Console.ReadLine();
            while (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Поле должно быть заполнено");
                title = Console.ReadLine();
            }

            Console.Write("Введите автора: ");
            string author = Console.ReadLine();
            while (string.IsNullOrEmpty(author))
            {
                Console.WriteLine("Поле должно быть заполнено");
                title = Console.ReadLine();
            }

            Console.Write("Введите жанр: ");
            string genre = Console.ReadLine();
            while (string.IsNullOrEmpty(genre))
            {
                Console.WriteLine("Поле должно быть заполнено");
                title = Console.ReadLine();
            }

            manager.AddBook(title, author, genre);
            Console.WriteLine("\nКнига добавлена!");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// удалить книгу
        /// </summary>
        /// <param name="manager">менеджер</param>
        static void DeleteBookMenu(LibraryManager manager)
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КНИГИ ===\n");

            List<Book> books = manager.Books.ToList();
            if (!books.Any())
            {
                Console.WriteLine("Нет книг для удаления.");
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {books[i].Title} — {books[i].Author}");
            }

            Console.Write("\nВведите номер книги для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= books.Count)
            {
                manager.DeleteBook(books[choice - 1]);
                Console.WriteLine("\nКнига удалена!");
            }
            else
            {
                Console.WriteLine("\nНеверный выбор.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// доюавить читателя
        /// </summary>
        /// <param name="manager">менеджер</param>
        static void AddReaderMenu(LibraryManager manager)
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

            manager.AddReader(name, address);
            Console.WriteLine("\nЧитатель добавлен!");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

}
