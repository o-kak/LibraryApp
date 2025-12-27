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

            try
            {
                _viewManager = new ViewManager();
                System.Diagnostics.Debug.WriteLine("WPF приложение запущено");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Не удалось запустить приложение: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _viewManager?.Shutdown();
            base.OnExit(e);
        }

    }
}
