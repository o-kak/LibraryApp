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
        private BindingList<ReaderEventArgs> _readers;
        private string _newReaderName;
        private string _newReaderAddress;
        private ReaderEventArgs _selectedReader;
        private string _searchName;

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

        public ReaderViewModel(IReaderService readerService)
        {
            _readerService = readerService;
            Readers = new BindingList<ReaderEventArgs>();

            // Подписываемся на событие изменения данных
            _readerService.DataChanged += OnReaderDataChanged;

            LoadReadersCommand = new RelayCommand(LoadReaders);
            DeleteReaderCommand = new RelayCommand(DeleteReader, () => SelectedReader != null);
            AddReaderCommand = new RelayCommand(AddReader, CanAddReader);
            UpdateReaderCommand = new RelayCommand(UpdateReader, () => SelectedReader != null);

            LoadReaders();
        }

        private void OnReaderDataChanged(IEnumerable<Reader> updatedReaders)
        {
            // Обновляем коллекцию при изменениях из сервиса
            System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
            {
                RefreshReaders(updatedReaders);
            });
        }

        // Конвертация Model -> ReaderEventArgs
        private ReaderEventArgs ConvertToEventArgs(Reader model)
        {
            return new ReaderEventArgs
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address
            };
        }

        // Конвертация ReaderEventArgs -> Model
        private Reader ConvertToModel(ReaderEventArgs dto)
        {
            return new Reader
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address
            };
        }

        private void RefreshReaders(IEnumerable<Reader> modelReaders)
        {
            Readers.Clear();
            foreach (var reader in modelReaders)
            {
                Readers.Add(ConvertToEventArgs(reader));
            }
        }

        private void LoadReaders()
        {
            var allReaders = _readerService.GetAllReaders();
            RefreshReaders(allReaders);
        }


        private void DeleteReader()
        {
            if (SelectedReader != null)
            {
                // Удаляем через сервис (он вызовет событие DataChanged)
                _readerService.Delete(SelectedReader.Id);
                SelectedReader = null;
            }
        }

        private void UpdateReader()
        {
            if (SelectedReader != null)
            {
                // Конвертируем DTO в Model и обновляем в сервисе
                var model = ConvertToModel(SelectedReader);
                _readerService.Update(model);
                // Событие DataChanged обновит список автоматически
            }
        }


        private void AddReader()
        {
            if (!CanAddReader())
            {
                return;
            }

            var newReader = new Reader
            {
                Name = NewReaderName.Trim(),
                Address = NewReaderAddress.Trim()
            };

            // Добавляем через сервис (он вызовет событие DataChanged)
            _readerService.Add(newReader);

            // Очищаем поля
            NewReaderName = string.Empty;
            NewReaderAddress = string.Empty;
        }

        private bool CanAddReader()
        {
            return !string.IsNullOrWhiteSpace(NewReaderName) &&
                   !string.IsNullOrWhiteSpace(NewReaderAddress);
        }


        // Не забываем отписаться от событий
        public void Dispose()
        {
            _readerService.DataChanged -= OnReaderDataChanged;
        }
    }
}
