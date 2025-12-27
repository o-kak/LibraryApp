using Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfView
{
    public class View : Window
    {
        public event Action<View> ClosedEvent;
        public View()
        {
        }

        protected ViewModelBase ViewModel => DataContext as ViewModelBase;


        protected override void OnClosed(EventArgs e)
        {
            ClosedEvent?.Invoke(this);
            (ViewModel as IDisposable)?.Dispose();
            base.OnClosed(e);
        }
    }
}
