using BusinessLogic;
using Model;
using Ninject;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    public class BookViewModel : ViewModelBase
    {
        private readonly IBookService _bookService;
        private readonly VMManager _vmManager;

  
        private string _title;
        private string _genre;      
        private string _author;

        /// <summary>
        /// Название книги.
        /// При изменении значения автоматически уведомляет об обновлении состояния команды сохранения.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Жанр книги.
        /// При изменении значения автоматически уведомляет об обновлении состояния команды сохранения.
        /// </summary>
        public string Genre
        {
            get => _genre;
            set
            {
                if (_genre != value)
                {
                    _genre = value;
                    OnPropertyChanged();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Автор книги.
        /// При изменении значения автоматически уведомляет об обновлении состояния команды сохранения.
        /// </summary>
        public string Author
        {
            get => _author;
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged();
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }

        public BookViewModel(VMManager vmManager)
        {
            _bookService = new StandardKernel(new SimpleConfigModule()).Get<BookService>(); ;
            _vmManager = vmManager;

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        /// <summary>
        /// Определяет, может ли команда сохранения быть выполнена.
        /// Команда доступна только когда все обязательные поля (название, жанр, автор) заполнены.
        /// </summary>
        /// <returns>true, если все обязательные поля заполнены; иначе false.</returns>
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
               !string.IsNullOrWhiteSpace(Genre) &&
               !string.IsNullOrWhiteSpace(Author);

        }

        /// <summary>
        /// Сохраняет новую книгу в базу данных.
        /// Создает объект Book на основе введенных данных и передает его в сервис книг.
        /// После успешного сохранения закрывает текущее окно.
        /// </summary>
        private void Save()
        {
            var bookModel = new Book
            {
                Id = 0,
                Title = Title,
                Genre = Genre,
                Author = Author,
                IsAvailable = true
            };

            _bookService.Add(bookModel);
            _vmManager.CloseCurrentView();
        }

        /// <summary>
        /// Освобождает ресурсы, используемые BookViewModel.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
