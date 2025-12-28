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

        private BookEventArgs _newBook;

        public BookEventArgs NewBook
        {
            get => _newBook;
            set
            {
                if (_newBook != value)
                {
                    _newBook = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        public BookViewModel(VMManager vmManager)
        {
            _bookService = _bookService = new StandardKernel(new SimpleConfigModule()).Get<BookService>(); ;
            _vmManager = vmManager;

            NewBook = new BookEventArgs();

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);

        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(NewBook.Title) &&
               !string.IsNullOrWhiteSpace(NewBook.Genre) &&
               !string.IsNullOrWhiteSpace(NewBook.Author);

        }

        private void Save()
        {
            var bookModel = new Book
            {
                Id = 0,
                Title = NewBook.Title,
                Genre = NewBook.Genre,
                Author = NewBook.Author,
            };

            _bookService.Add(bookModel);
            _vmManager.CloseCurrentView();
        }
        private void Cancel()
        {
            _vmManager.CloseCurrentView();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
