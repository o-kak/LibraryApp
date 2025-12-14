using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter.ViewModel
{
    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда быть выполнена.
        /// </summary>
        /// <param name="parameter">Параметр, передаваемый в команду (не используется).</param>
        /// <returns>true, если команда может быть выполнена; иначе false.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр, передаваемый в команду (не используется).</param>
        public void Execute(object parameter) => _execute();

        /// <summary>
        /// Событие, которое вызывается при изменении состояния выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}
