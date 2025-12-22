using BusinessLogic;
using Model;
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
    public class ReaderViewModel : ViewModelBase, IDisposable
    {
        private readonly IReaderService _readerService;
        private readonly ILoan _loanService;
        private BindingList<ReaderEventArgs> _readers;


        private ReaderEventArgs _selectedReader;

        private string _newReaderName;
        private string _newReaderAddress;

        private BindingList<BookEventArgs> _readerBooks;

        public BindingList<ReaderEventArgs> Readers
        {
            get => _readers;
            set
            {
                _readers = value;
                OnPropertyChanged();
            }
        }
        public BindingList<BookEventArgs> ReaderBooks
        {
            get => _readerBooks;
            set
            {
                _readerBooks = value;
                OnPropertyChanged();
            }
        }

        public string NewReaderName
        {
            get => _newReaderName;
            set { _newReaderName = value; OnPropertyChanged(); }
        }

        public string NewReaderAddress
        {
            get => _newReaderAddress;
            set { _newReaderAddress = value; OnPropertyChanged(); }
        }

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader != value)
                {
                    _selectedReader = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanUpdateReader));
                }
            }
        }
        public bool CanUpdateReader => SelectedReader != null;
        public bool CanDeleteReader => SelectedReader != null;
        public bool CanAddReader => !string.IsNullOrWhiteSpace(NewReaderName) &&
                                    !string.IsNullOrWhiteSpace(NewReaderAddress);

        public ICommand StartUpCommand { get; }
        public ICommand DeleteReaderCommand { get; }
        public ICommand AddReaderCommand { get; }
        public ICommand UpdateReaderCommand { get; }
        public ICommand ReadeByIdCommand { get; }
        public ICommand GetBorrowedBooksCommand { get; }



        public ReaderViewModel()
        {
            _readerService = ServiceLocator.ReaderService;
            _loanService = ServiceLocator.LoanService;

            Readers = new BindingList<ReaderEventArgs>();
            ReaderBooks = new BindingList<BookEventArgs>();

            _readerService.DataChanged += OnModelDataChanged;

            StartUpCommand = new RelayCommand(StartUp);
            AddReaderCommand = new RelayCommand(AddReader, () => CanAddReader);
            DeleteReaderCommand = new RelayCommand(DeleteReader, () => SelectedReader != null);
            UpdateReaderCommand = new RelayCommand(UpdateReader, () => SelectedReader != null);
            ReadeByIdCommand = new RelayCommand(ReadById, () => SelectedReader != null);
            GetBorrowedBooksCommand = new RelayCommand(GetBorrowedBooks, () => SelectedReader != null);

            StartUp();
        }


        private void StartUp() 
        {
            _readerService.InvokeDataChanged();
        }

        private void OnModelDataChanged(IEnumerable<Reader> readers)
        {
            var readerList = readers.ToList();
            SyncModelToDTO(readerList);
        }

        private void SyncModelToDTO(List<Reader> modelReaders)
        {
            var dtos = new List<ReaderEventArgs>();
            foreach (var model in modelReaders.OrderBy(r => r.Name))
            {
                dtos.Add(new ReaderEventArgs
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address
                });
            }

            Readers = new BindingList<ReaderEventArgs>(dtos);
        }

        private Reader ConvertDTOToModel(ReaderEventArgs dto)
        {
            return new Reader
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address
            };
        }

        private void AddReader()
        {
            var reader = new Reader
            {
                Name = NewReaderName?.Trim(),
                Address = NewReaderAddress?.Trim()
            };

            _readerService.Add(reader);

            NewReaderName = string.Empty;
            NewReaderAddress = string.Empty;
        }

        private void DeleteReader()
        {
            if (SelectedReader == null) return;
            if (SelectedReader != null)
            {

                var books = _loanService.GetReadersBorrowedBooks(SelectedReader.Id).ToList();
                if (books.Any())
                {
                    foreach (var book in books)
                    {
                        _loanService.ReturnBook(book.Id, SelectedReader.Id);
                    }
                }

                _readerService.Delete(SelectedReader.Id);
                SelectedReader = null;
                ReaderBooks.Clear();
            }
        }

        private void UpdateReader() 
        {
            if (SelectedReader != null)
            {
                var reader = new Reader
                {
                    Id = SelectedReader.Id,
                    Name = SelectedReader.Name?.Trim(),
                    Address = SelectedReader.Address?.Trim()
                };

                _readerService.Update(reader);
            }
        }

        /// <summary>
        /// чтение пользователя по ID
        /// </summary>
        private void ReadById()
        {
            if (SelectedReader != null)
            {
                var reader = _readerService.GetReader(SelectedReader.Id);
                var args = new ReaderEventArgs()
                {
                    Id = SelectedReader.Id,
                    Name = reader.Name,
                    Address = reader.Address
                };

                // В реальном приложении здесь можно открыть профиль
                // Например, установить свойство для отображения деталей
            }
        }

        /// <summary>
        /// получить книги на руках у читателя
        /// </summary>
        private void GetBorrowedBooks()
        {
            if (SelectedReader != null)
            {
                LoadReadersBorrowedBooks(SelectedReader.Id);
            }
        }

        /// <summary>
        /// Загрузить книги читателя
        /// </summary>
        private void LoadReadersBorrowedBooks(int id)
        {
            var books = _loanService.GetReadersBorrowedBooks(id).ToList();
            var args = new List<BookEventArgs>();
            foreach (var book in books)
            {
                args.Add(new BookEventArgs()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ReaderId = id
                });
            }

            ReaderBooks.Clear();
            foreach (var arg in args)
            {
                ReaderBooks.Add(arg);
            }
        }

        public void Dispose()
        {
            _readerService.DataChanged -= OnModelDataChanged;
        }

    }
}
