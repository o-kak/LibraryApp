using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsView
{
    public partial class AddBookForm : Form
    {
        private readonly BookView _bookView;
        public AddBookForm(BookView bookView)
        {
            _bookView = bookView;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string bookTitle = this.TitleText.Text;
            string bookAuthor = this.AuthorText.Text;
            string bookGenre = this.GenreText.Text;

            if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookAuthor) || string.IsNullOrWhiteSpace(bookGenre))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var bookEventArgs = new BookEventArgs
            {
                Id = 0,
                Title = bookTitle,
                Author = bookAuthor,
                Genre = bookGenre,
                ReaderId = null
            };

            _bookView.TriggerAddData(bookEventArgs);
            this.Close();
        }
    }
}
