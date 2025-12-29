using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_VIEW
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ViewManager _viewManager;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _viewManager = new ViewManager();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            _viewManager?.Shutdown();
            base.OnExit(e);
        }

    }
}
