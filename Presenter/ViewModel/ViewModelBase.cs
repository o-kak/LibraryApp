using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Presenter.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged для указанного свойства.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства. Если не указано, используется имя вызывающего свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Освобождает ресурсы, используемые ViewModelBase
        /// </summary>
        public virtual void Dispose()
        {
            PropertyChanged = null;
        }
    }

}
