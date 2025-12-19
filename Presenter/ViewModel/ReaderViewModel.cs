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
        public bool CanDeleteReader => SelectedReader != null;
        public bool CanAddReader => !string.IsNullOrWhiteSpace(NewReaderName) &&
                                    !string.IsNullOrWhiteSpace(NewReaderAddress);

        public ICommand LoadReadersCommand { get; }
        public ICommand DeleteReaderCommand { get; }
        public ICommand AddReaderCommand { get; }
        public ICommand UpdateReaderCommand { get; }
        public ICommand ShowReaderProfileCommand { get; }
        public ICommand ShowBorrowedBooksCommand { get; }



        public ReaderViewModel(IReaderService readerService, ILoan loanService)
        {
            _readerService = readerService;
            _loanService = loanService;

            Readers = new BindingList<ReaderEventArgs>();

            _readerService.DataChanged += OnReaderDataChanged;
            _readerService.InvokeDataChanged();

            LoadReadersCommand = new RelayCommand(() => _readerService.InvokeDataChanged());
            AddReaderCommand = new RelayCommand(AddReader, () => CanAddReader);
            DeleteReaderCommand = new RelayCommand(DeleteReader, () => SelectedReader != null);
            UpdateReaderCommand = new RelayCommand(UpdateReader, () => SelectedReader != null);
            ShowReaderProfileCommand = new RelayCommand(ShowReaderProfile, () => SelectedReader != null);
            ShowBorrowedBooksCommand = new RelayCommand(ShowBorrowedBooks, () => SelectedReader != null);

            LoadReaders();
        }


        private void OnReaderDataChanged(IEnumerable<Reader> readers)
        {
            RefreshReaders(readers);
        }

        private void RefreshReaders(IEnumerable<Reader> readers)
        {
            var previousSelectedId = SelectedReader?.Id;

            Readers.Clear();
            foreach (var reader in readers)
            {
                var readerDto = new ReaderEventArgs
                {
                    Id = reader.Id,
                    Name = reader.Name,
                    Address = reader.Address
                };
                Readers.Add(readerDto);

            }
        }

        private void AddReader()
        {
            if (!CanAddReader) return;

            try
            {
                var reader = new Reader
                {
                    Id = 0,
                    Name = NewReaderName.Trim(),
                    Address = NewReaderAddress.Trim()
                };

                _readerService.Add(reader);
                NewReaderName = string.Empty;
                NewReaderAddress = string.Empty;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Ошибка при добавлении читателя: {ex.Message}");

            }
        }

        private void DeleteReader()
        {
            if (!CanDeleteReader) return;

            var result = ShowConfirmationDialog();
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    _readerService.Delete(SelectedReader.Id);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Ошибка при удалении читателя: {ex.Message}");
                }
            }
        }

        private void UpdateReader() 
        {
            if (!CanUpdateReader) return;

            try
            {
                var reader = new Reader
                {
                    Id = SelectedReader.Id,
                    Name = SelectedReader.Name,
                    Address = SelectedReader.Address
                };
                _readerService.Update(reader);
            }
            catch (Exception ex) 
            {
                ShowErrorMessage($"Ошибка при обновлении читателя: {ex.Message}");
            }
        }

        private void ShowReaderProfile() 
        {
            if (SelectedReader != null) 
            {

            }

        }


    }
}
