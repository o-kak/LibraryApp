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
            _bookService = _bookService = new StandardKernel(new SimpleConfigModule()).Get<BookService>(); ;
            _vmManager = vmManager;

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
               !string.IsNullOrWhiteSpace(Genre) &&
               !string.IsNullOrWhiteSpace(Author);

        }

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

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
