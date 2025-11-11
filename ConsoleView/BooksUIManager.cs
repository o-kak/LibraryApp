using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Model;

namespace ConsoleView
{
    internal class BooksUIManager
    {
        private IBookService BookService { get; set; }
        private BookAuthorFilter BookAuthorFilter { get; set; }
        private BookGenreFilter BookGenreFilter { get; set; }
        private ILoan LoanService { get; set; }
        public BooksUIManager(IBookService bookService, BookAuthorFilter bookAuthorFilter, BookGenreFilter bookGenreFilter, ILoan loanService)
        {
            BookService = bookService;
            BookAuthorFilter = bookAuthorFilter;
            BookGenreFilter = bookGenreFilter;
            LoanService = loanService;
        }

        /// <summary>
        /// вывести список книг
        /// </summary>
        /// <param name="books">список книг</param>
        public void ShowBooks(IEnumerable<Book> books)
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
                ShowBooks(BookGenreFilter.Filter(genre ?? ""));
            }
            else if (key == ConsoleKey.D2)
            {
                Console.Write("\nВведите автора: ");
                string author = Console.ReadLine();
                while (string.IsNullOrEmpty(author))
                {
                    Console.WriteLine("Введенное значение не должно быть пустым");
                }
                ShowBooks(BookAuthorFilter.Filter(author ?? ""));
            }
        }

        /// <summary>
        /// меню действий с книгами
        /// </summary>
        public void ShowBooksMenu()
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
                        ShowBooks(BookService.GetAllBooks());
                        break;
                    case ConsoleKey.D2:
                        ShowBooks(LoanService.GetAvailableBooks());
                        break;
                    case ConsoleKey.D3:
                        ShowBooks(LoanService.GetBorrowedBooks());
                        break;
                    case ConsoleKey.D4:
                        AddBookMenu();
                        break;
                    case ConsoleKey.D5:
                        DeleteBookMenu();
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

            BookService.AddBook(title, author, genre);
            Console.WriteLine("\nКнига добавлена!");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// меню удаления книги
        /// </summary>
        public void DeleteBookMenu()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КНИГИ ===\n");

            List<Book> books = BookService.GetAllBooks().ToList();
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
                BookService.DeleteBook(books[choice - 1].Id);
                Console.WriteLine("\nКнига удалена!");
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
