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
        private BookService bookService;
        public AddBookForm(BookService bookService)
        {
            this.bookService = bookService;
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
            bookService.AddBook(bookTitle, bookAuthor, bookGenre);
            var books = bookService.GetAllBooks().ToList();

            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.UpdateBooksListView(books);
            }
            this.Close();
        }
    }
}
