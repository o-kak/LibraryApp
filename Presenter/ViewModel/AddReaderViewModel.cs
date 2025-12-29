using BusinessLogic;
using Model;
using Ninject;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    public class AddReaderViewModel : ViewModelBase
    {
        private readonly IReaderService _readerService;
        private readonly VMManager _vmManager;

        private string _name;
        private string _address;
        private int _id;

        /// <summary>
        /// ФИО читателя.
        /// При изменении значения автоматически уведомляет об обновлении состояния команды сохранения.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                    if (SaveCommand is RelayCommand saveCmd)
                        saveCmd.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Адрес читателя.
        /// При изменении значения автоматически уведомляет об обновлении состояния команды сохранения.
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                    if (SaveCommand is RelayCommand saveCmd)
                        saveCmd.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Идентификатор читателя. Для нового читателя всегда равен 0.
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Команда для сохранения нового читателя в базу данных.
        /// </summary>
        public ICommand SaveCommand { get; }

        public AddReaderViewModel(VMManager vmManager)
        {
            _readerService = new StandardKernel(new SimpleConfigModule()).Get<ReaderService>();
            _vmManager = vmManager;

            Id = 0;
            Name = string.Empty;
            Address = string.Empty;

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        /// <summary>
        /// Определяет, может ли команда сохранения быть выполнена.
        /// Команда доступна только когда все обязательные поля (имя и адрес) заполнены.
        /// </summary>
        /// <returns>true, если все обязательные поля заполнены; иначе false.</returns>
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Address);
        }

        /// <summary>
        /// Сохраняет нового читателя в базу данных.
        /// Создает объект Reader на основе введенных данных и передает его в сервис читателей.
        /// После успешного сохранения закрывает текущее окно.
        /// </summary>
        private void Save()
        {
            var readerModel = new Reader
            {
                Id = 0,
                Name = Name.Trim(),
                Address = Address.Trim()
            };

            _readerService.Add(readerModel);
            _vmManager.CloseCurrentView();
        }

        /// <summary>
        /// Освобождает ресурсы, используемые AddReaderViewModel.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
