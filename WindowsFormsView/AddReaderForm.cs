using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsView
{
    public partial class AddReaderForm : Form
    {

        private readonly ReaderView _readerView;
        private readonly LoanView _loanView;

        public AddReaderForm(ReaderView readerView, LoanView loanView)
        {
            _readerView = readerView;
            _loanView = loanView;

            InitializeComponent();
        }

        /// <summary>
        /// Метод создающий нового пользователя
        /// </summary>
        private void SaveReaderButton_Click(object sender, EventArgs e)
        {
            
            string readerName = ReaderNamTextBox.Text;
            string readerAdress = ReaderAdressTextBox.Text;

            if (string.IsNullOrWhiteSpace(readerName) || string.IsNullOrWhiteSpace(readerAdress))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var readerEventArgs = new ReaderEventArgs
            {
                Id = 0, 
                Name = readerName,
                Address = readerAdress
            };
            _readerView.TriggerAddData(readerEventArgs);
            _loanView.ShowMessage($"Читатель '{readerName}' успешно добавлен.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
