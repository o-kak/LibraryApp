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
    public class UpdateReaderViewModel : ViewModelBase
    {
        private readonly IReaderService _readerService;
        private readonly VMManager _vmManager;


        private ReaderEventArgs _selectedReader;

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader != value)
                {
                    _selectedReader = value;
                    OnPropertyChanged();
                    if (SaveCommand is RelayCommand saveCmd)
                        saveCmd.RaiseCanExecuteChanged();

                }
            }
        }

        public ICommand SaveCommand { get; }

        public UpdateReaderViewModel(VMManager vmManager, ReaderEventArgs existingReader)
        {
            _readerService = new StandardKernel(new SimpleConfigModule()).Get<ReaderService>();
            _vmManager = vmManager;

            SelectedReader = existingReader;

            SaveCommand = new RelayCommand(Save,() => CanSave());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(SelectedReader.Name) &&
                !string.IsNullOrWhiteSpace(SelectedReader.Address);
        }

        private void Save()
        {
            var readerModel = new Reader
            {
                Id = SelectedReader.Id,
                Name = SelectedReader.Name.Trim(),
                Address = SelectedReader.Address.Trim()
            };

            _readerService.Update(readerModel);

            _vmManager.CloseCurrentView();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
