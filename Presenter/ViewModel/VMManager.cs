using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter.ViewModel
{
    public class VMManager : ViewModelBase
    {
        public event EventHandler<ViewModelBase> ViewModelChanged;

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
                ViewModelChanged?.Invoke(this, value);
            }
        }
    }
}
