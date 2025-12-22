using BusinessLogic;
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

        public VMManager()
        {
         
        }

        public void Start() 
        {
            CurrentViewModel = new ViewModelMAin(this);
        }
        public BookViewModel CreateBookViewModel()
        {
            var vm = new BookViewModel();
            CurrentViewModel = vm; 
            return vm;
        }

        public ReaderViewModel CreateReaderViewModel()
        {
            var vm = new ReaderViewModel();
            CurrentViewModel = vm;
            return vm;
        }

        public LoanViewModel CreateLoanViewModel()
        {
            var vm = new LoanViewModel();
            CurrentViewModel = vm;
            return vm;
        }
       
    }
}
