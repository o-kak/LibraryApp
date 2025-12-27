using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presenter.ViewModel;



namespace WPF_VIEW
{
    public partial class BaseView : Window
    {
        public event Action<BaseView> ClosedEvent;
        protected ViewModelBase ViewModel => DataContext as ViewModelBase;


        protected override void OnClosed(EventArgs e)
        {
            ClosedEvent?.Invoke(this);
            (ViewModel as IDisposable)?.Dispose();
            base.OnClosed(e);
        }

    }
}
