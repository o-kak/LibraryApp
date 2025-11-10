using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;
using Ninject;

namespace WindowsFormsView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            Application.Run(ninjectKernel.Get<Form1>());
        }
    }
}
