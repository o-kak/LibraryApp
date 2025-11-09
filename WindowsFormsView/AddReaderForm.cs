using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using BusinessLogic;

namespace WindowsFormsView
{
    public partial class AddReaderForm : Form
    {
        private BookService bookService;
        private ReaderService readerService; 
        private LoanService loanService;

        public AddReaderForm(BookService bookService, ReaderService readerService, LoanService loanService)
        {
            this.bookService = bookService;
            this.readerService = readerService;
            this.loanService = loanService;
            List<Book> books = bookService.GetAllBooks().ToList();
            InitializeComponent();
            FillBorrowedBooksCheckedListBox(books);
        }
        private void FillBorrowedBooksCheckedListBox(List<Book> books)
        {
            BorrowedBooksCheckedListBox.Items.Clear();
            foreach (var book in books)
            {
                BorrowedBooksCheckedListBox.Items.Add(book); 
            }
        }
        private void SaveReaderButton_Click(object sender, EventArgs e)
        {
            
            string readerName = ReaderNamTextBox.Text;
            string readerAdress = ReaderAdressTextBox.Text;

            if (string.IsNullOrWhiteSpace(readerName) || string.IsNullOrWhiteSpace(readerAdress))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            readerService.AddReader(readerName, readerAdress);   

            bool hasErrors = false;
            List<Reader> readers = readerService.GetAllReaders().ToList();

            if (readers.Count == 0)
            {
                MessageBox.Show("Не удалось получить список читателей.");
                return;
            }
            if (BorrowedBooksCheckedListBox.CheckedItems.Count > 0)
            {
                foreach (var item in BorrowedBooksCheckedListBox.CheckedItems)
                {
                    Book selectedBook = item as Book;
                    if (selectedBook != null)
                    {
                        try
                        {
                            loanService.GiveBook(selectedBook.Id, readers.Last().Id);
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            hasErrors = true;
                            readerService.DeleteReader(readers.Last().Id);
                        }
                    }
                }
            }

            if (!hasErrors)
            {
                Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                if (mainForm != null)
                {
                    mainForm.UpdateReaderListView(readers); 
                }

                this.Close();
            }
        }
    }
}
