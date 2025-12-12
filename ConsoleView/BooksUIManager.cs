using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ConsoleView
{
    public class BooksUIManager : IBookView
    {
        public event Action<EventArgs> AddDataEvent;
        public event Action<int> DeleteDataEvent;
        public event Action<string> FilterDataByAuthorEvent;
        public event Action<string> FilterDataByGenreEvent;
        public event Action GetAvailableBooksEvent;
        public event Action GetBorrowedBooksEvent;
        public event Action StartupEvent;

        public void Start()
        {
            StartupEvent?.Invoke();
        }

        /// <summary>
        /// вывести список книг
        /// </summary>
        /// <param name="books">список книг</param>
        public void ShowBooks(IEnumerable<BookEventArgs> books)
        {
            Console.Clear();
            Console.WriteLine("=== СПИСОК КНИГ ===\n");

            foreach (var book in books)
            {
                Console.WriteLine($"Id: {book.Id} {book.Title} — {book.Author} [{book.Genre}] ");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// фильтрация книг
        /// </summary>
        public void FilterBooksMenu()
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
                FilterDataByGenreEvent?.Invoke(genre);
            }
            else if (key == ConsoleKey.D2)
            {
                Console.Write("\nВведите автора: ");
                string author = Console.ReadLine();
                while (string.IsNullOrEmpty(author))
                {
                    Console.WriteLine("Введенное значение не должно быть пустым");
                }
                FilterDataByAuthorEvent?.Invoke(author);
            }
        }

        /// <summary>
        /// меню действий с книгами
        /// </summary>
        public void Redraw(IEnumerable<EventArgs> data)
        {
            ConsoleKey key;
            IEnumerable<BookEventArgs> books = data as IEnumerable<BookEventArgs>;
            ShowBooks(books);
            do
            {
                Console.Clear();
                Console.WriteLine("=== КНИГИ ===\n");
                Console.WriteLine("[1] Показать доступные книги");
                Console.WriteLine("[2] Показать выданные книги");
                Console.WriteLine("[3] Добавить книгу");
                Console.WriteLine("[4] Удалить книгу");
                Console.WriteLine("[Esc] Назад");

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        GetAvailableBooksEvent?.Invoke();
                        break;
                    case ConsoleKey.D2:
                        GetBorrowedBooksEvent?.Invoke();
                        break;
                    case ConsoleKey.D3:
                        AddBookMenu();
                        break;
                    case ConsoleKey.D4:
                        DeleteBookMenu(data);
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        /// <summary>
        /// меню добавления книги
        /// </summary>
        public void AddBookMenu()
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

            Console.WriteLine("\nКнига добавлена!");
            AddDataEvent?.Invoke(new BookEventArgs()
            {
                Title = title,
                Author = author,
                Genre = genre
            });
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// меню удаления книги
        /// </summary>
        public void DeleteBookMenu(IEnumerable<EventArgs> data)
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КНИГИ ===\n");

            List<BookEventArgs> books = (data as IEnumerable<BookEventArgs>).ToList();
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
                Console.WriteLine("\nКнига удалена!");
                DeleteDataEvent?.Invoke(books[choice - 1].Id);
            }
            else
            {
                Console.WriteLine("\nНеверный выбор.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
