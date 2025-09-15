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

namespace WindowsFormsView
{
    public partial class AddReaderForm : Form
    {
        private LibraryManager _libraryManager;

        public AddReaderForm(LibraryManager libraryManager)
        {
            this._libraryManager = libraryManager;
            InitializeComponent();
            FillBorrowedBooksCheckedListBox();
        }
        private void FillBorrowedBooksCheckedListBox()
        {
            BorrowedBooksCheckedListBox.Items.Clear();
            foreach (var book in _libraryManager.Books)
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
            var readers = _libraryManager.Readers.ToList();

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
                            _libraryManager.GiveBook(selectedBook, readers.Last());
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            hasErrors = true;
                            _libraryManager.DeleteReader(readers.Last());
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
