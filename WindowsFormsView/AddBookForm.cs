using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;

namespace WindowsFormsView
{
    public partial class AddBookForm : Form
    {
        private LibraryManager __libraryManager;
        public AddBookForm(LibraryManager libraryManager)
        {
            this.__libraryManager = libraryManager;
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
            __libraryManager.AddBook(bookTitle, bookAuthor, bookGenre);
            var books = __libraryManager.Books.ToList();

            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.UpdateBooksListView(books);
            }
            this.Close();
        }
    }
}
