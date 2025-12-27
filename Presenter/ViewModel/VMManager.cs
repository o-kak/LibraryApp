using BusinessLogic;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter.ViewModel
{
    public class VMManager : ViewModelBase
    {
        public event Action<ViewModelBase> VMMReadyEvent;
        public event Action ViewModelClosedEvent;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel == value)
                    return;


                _currentViewModel = value;
                OnPropertyChanged();
                VMMReadyEvent?.Invoke(value);
            }
        }
        public void Start()
        {
            ShowMainView();
        }

        public void ShowMainView()
        {
            CurrentViewModel = new ViewModelMain(this);
            VMMReadyEvent?.Invoke(CurrentViewModel);
        }

        public BookViewModel CreateBookViewModel()
        {
            var vm = new BookViewModel(this);
            CurrentViewModel = vm;
            VMMReadyEvent?.Invoke(vm);
            return vm;
        }

        public ReaderViewModel CreateReaderViewModel(ReaderEventArgs existingReader = null)
        {
            var vm = new ReaderViewModel(this, existingReader);
            CurrentViewModel = vm;
            VMMReadyEvent?.Invoke(vm);
            return vm;
        }

        public ReturnGiveBookViewModel CreateReturnGiveBookViewModel()
        {
            var vm = new ReturnGiveBookViewModel();
            CurrentViewModel = vm;
            VMMReadyEvent?.Invoke(vm);
            return vm;
        }
        public void CloseCurrentView()
        {
            ViewModelClosedEvent?.Invoke();
            ShowMainView();
        }

    }
}
