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
    public class ReaderViewModel : ViewModelBase
    {
        private readonly IReaderService _readerService;
        private readonly VMManager _vmManager;


        private ReaderEventArgs _selectedReader;
        private bool _isEditMode;

        public ReaderEventArgs SelectedReader
        {
            get => _selectedReader;
            set
            {
                if (_selectedReader != value)
                {
                    _selectedReader = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title => _isEditMode ? "Редактирование читателя" : "Добавление читателя";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ReaderViewModel(VMManager vmManager, ReaderEventArgs existingReader = null)
        {
            _readerService = new StandardKernel(new SimpleConfigModule()).Get<ReaderService>();
            _vmManager = vmManager;

            IsEditMode = existingReader != null;
            SelectedReader = existingReader ?? new ReaderEventArgs()
            {
                Id = 0,
                Name = string.Empty,
                Address = string.Empty
            };

            SaveCommand = new RelayCommand(Save,() => CanSave());
            CancelCommand = new RelayCommand(Cancel);
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

            if (IsEditMode)
            {
                _readerService.Update(readerModel);
            }
            else
            {
                _readerService.Add(readerModel);
            }

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
