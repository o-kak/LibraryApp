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
        private LibraryManager _libraryManager;

        public AddReaderForm(LibraryManager libraryManager)
        {
            this._libraryManager = libraryManager;
            List<Book> books = libraryManager.GetAllBooks().ToList();
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

            _libraryManager.AddReader(readerName, readerAdress);   

            bool hasErrors = false;
            List<Reader> readers = _libraryManager.GetAllReaders().ToList();

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
                            _libraryManager.GiveBook(selectedBook.Id, readers.Last().Id);
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            hasErrors = true;
                            _libraryManager.DeleteReader(readers.Last().Id);
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
