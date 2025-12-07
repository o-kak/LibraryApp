using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Ninject;
using ConsoleView;

namespace Presenter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== Главное меню выбора UI =====");
                Console.WriteLine("1. Запустить Консольный режим (Console UI)");
                Console.WriteLine("2. Запустить WinForms режим (WinForms UI)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите вариант (0, 1 или 2): ");

                string choice = Console.ReadLine();

                IApplicationStartup startup = null;

                switch (choice)
                {
                    case "1":
                        // Создаем экземпляры всех нужных частей для Консоли
                        startup = new ConsoleBookView();
                        // Если ваш BookPresenter требует подписки на событие ConsoleBookView, 
                        // то запуск должен быть немного сложнее (см. ниже)
                        break;

                    case "2":
                        // Создаем экземпляры всех нужных частей для WinForms
                        // NOTE: Запуск WinForms из консоли может потребовать дополнительных настроек потока!
                        startup = new WinFormsBookForm();
                        break;

                    case "0":
                        running = false;
                        continue;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        Console.ReadKey();
                        continue;
                }

                if (startup != null)
                {
                    Console.Clear();
                    Console.WriteLine($"Запуск {startup.GetType().Name}...");

                    // Запускаем выбранный UI-слой
                    startup.Run();

                    // Возвращаемся в главное меню после завершения работы UI
                    Console.WriteLine("Сессия UI завершена. Возврат в главное меню.");
                    Console.ReadKey();
                }
            }
        }
    }
}
