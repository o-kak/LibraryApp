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

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Address);
        }

        private void Save()
        {
            var readerModel = new Reader
            {
                Id = Id,
                Name = Name.Trim(),
                Address = Address.Trim()
            };

            _readerService.Add(readerModel);
            _vmManager.CloseCurrentView();
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
