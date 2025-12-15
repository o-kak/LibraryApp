using BusinessLogic;
using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;                                       
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Presenter.ViewModel
{
    public class ReaderViewModel : ViewModelBase
    {
        private readonly IReaderService _readerService;
        private readonly ILoan _loanService;
        private BindingList<ReaderEventArgs> _readers;

        private ReaderEventArgs _selectedReader;

        private string _newReaderName;
        private string _newReaderAddress;

        private bool _isInitialized = false;

        public BindingList<ReaderEventArgs> Readers
        {
            get => _readers;
            set
            {
                _readers = value;
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

        public ICommand LoadReadersCommand { get; }
        public ICommand DeleteReaderCommand { get; }
        public ICommand AddReaderCommand { get; }
        public ICommand UpdateReaderCommand { get; }
        public ICommand ShowReaderProfileCommand { get; }
        public ICommand ShowBorrowedBooksCommand { get; }
            
        

        public ReaderViewModel(IReaderService readerService)
        {
            _readerService = readerService;
            Readers = new BindingList<ReaderEventArgs>();

            _readerService.DataChanged += OnInitialDataLoad;
            _readerService.InvokeDataChanged();

            LoadReadersCommand = new RelayCommand(LoadReaders);
            AddReaderCommand = new RelayCommand(AddReader, CanAddReader);
            DeleteReaderCommand = new RelayCommand(DeleteReader, () => SelectedReader != null);
            UpdateReaderCommand = new RelayCommand(UpdateReader, () => SelectedReader != null);
            ShowReaderProfileCommand = new RelayCommand(ShowReaderProfile, () => SelectedReader != null);
            ShowBorrowedBooksCommand = new RelayCommand(ShowBorrowedBooks, () => SelectedReader != null);

            LoadReaders();
        }


        private void OnInitialDataLoad(IEnumerable<Reader> readers)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _readerService.DataChanged -= OnInitialDataLoad; // Отписываемся!

                // Загружаем начальные данные
                RefreshReaders(readers);
            }
        }

        private void RefreshReaders(IEnumerable<Reader> readers)
        {
            Readers.Clear();
            foreach (var reader in readers)
            {
                Readers.Add(new ReaderEventArgs
                {
                    Id = reader.Id,
                    Name = reader.Name,
                    Address = reader.Address
                });
            }
        }



    }
}
