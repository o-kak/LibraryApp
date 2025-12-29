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
        /// <summary>
        /// Событие, возникающее когда новая ViewModel готова к отображению.
        /// </summary>
        public event Action<ViewModelBase> VMMReadyEvent;

        /// <summary>
        /// Событие, возникающее когда текущая ViewModel должна быть закрыта.
        /// </summary>
        public event Action ViewModelClosedEvent;

        private ViewModelBase _currentViewModel;

        /// <summary>
        /// Текущая активная ViewModel.
        /// При изменении значения вызывает событие VMMReadyEvent для уведомления о новой ViewModel.
        /// </summary>
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


        /// <summary>
        /// Инициализирует менеджер ViewModel и показывает главное окно.
        /// </summary>
        public void Start()
        {
            ShowMainView();
        }

        /// <summary>
        /// Создает и устанавливает ViewModel главного окна как текущую.
        /// </summary>
        public void ShowMainView()
        {
            CurrentViewModel = new ViewModelMain(this);
        }

        /// <summary>
        /// Создает ViewModel для добавления новой книги.
        /// </summary>
        /// <returns>Созданная BookViewModel.</returns>
        public BookViewModel CreateBookViewModel()
        {
            var vm = new BookViewModel(this);
            CurrentViewModel = vm;
            return vm;
        }

        /// <summary>
        /// Создает ViewModel для редактирования существующего читателя.
        /// </summary>
        /// <param name="existingReader">Существующий читатель для редактирования.</param>
        /// <returns>Созданная UpdateReaderViewModel.</returns>
        public UpdateReaderViewModel CreateUpdateReaderViewModel(ReaderEventArgs existingReader)
        {
            var vm = new UpdateReaderViewModel(this, existingReader);
            CurrentViewModel = vm;
            return vm;
        }

        /// <summary>
        /// Создает ViewModel для добавления нового читателя.
        /// </summary>
        /// <returns>Созданная AddReaderViewModel.</returns>
        public AddReaderViewModel CreateAddReaderViewModel() 
        {
            var vm = new AddReaderViewModel(this);
            CurrentViewModel = vm;
            return vm;
        }

        /// <summary>
        /// Создает ViewModel для выдачи или возврата книги.
        /// </summary>
        /// <returns>Созданная ReturnGiveBookViewModel.</returns>
        public ReturnGiveBookViewModel CreateReturnGiveBookViewModel()
        {
            var vm = new ReturnGiveBookViewModel();
            CurrentViewModel = vm;
            return vm;
        }

        /// <summary>
        /// Закрывает текущую ViewModel и вызывает соответствующее событие.
        /// </summary>
        public void CloseCurrentView()
        {
            ViewModelClosedEvent?.Invoke();
        }

    }
}
