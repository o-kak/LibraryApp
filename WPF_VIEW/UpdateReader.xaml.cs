using Presenter.ViewModel;
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

namespace WPF_VIEW
{
    /// <summary>
    /// Логика взаимодействия для UpdateReader.xaml
    /// </summary>
    public partial class UpdateReader : Window
    {

        public UpdateReader()
        {
            InitializeComponent();

        }

        public UpdateReader(UpdateReaderViewModel readerViewModel) : this() 
        {
            DataContext = readerViewModel;
        }
    }
}
