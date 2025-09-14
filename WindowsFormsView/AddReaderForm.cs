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
        LibraryManager libraryManager = new LibraryManager();
        public AddReaderForm()
        {
            InitializeComponent();


            // Пример добавления книг
            libraryManager.AddBook("1984", "George Orwell", "Dystopian");
            libraryManager.AddBook ("Brave New World", "Aldous Huxley", "Science Fiction");
            libraryManager.AddBook("Fahrenheit 451", "Ray Bradbury", "Dystopian");

            FillBorrowedBooksCheckedListBox();
        }

        private void FillBorrowedBooksCheckedListBox()
        {
            BorrowedBooksCheckedListBox.Items.Clear(); // Очистка текущих элементов

            foreach (var book in libraryManager.Books) // Предполагается, что _libraryManager инициализирован
            {
                BorrowedBooksCheckedListBox.Items.Add(book.Title); // Добавление названия книги
            }
        } 

        private void SaveReaderButton_Click(object sender, EventArgs e)
        {
            string readerName = ReaderNamTextBox.Text;
            string readerAdress = ReaderAdressTextBox.Text;

            libraryManager.AddReader(readerName, readerAdress);
            Reader currentReader = libraryManager.Readers.Last();
            if (BorrowedBooksCheckedListBox.CheckedItems.Count > 0) 
            {
                foreach (var item in BorrowedBooksCheckedListBox.CheckedItems) 
                {
                    Book selectedBook = item as Book;
                    if (selectedBook != null) 
                    {
                        try 
                        {
                            libraryManager.GiveBook(selectedBook, currentReader);
                        }

                        catch(InvalidProgramException ex) 
                        {
                            MessageBox.Show(ex.Message);   
                        }
                    }
                }
            }
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.UpdateReaderListView(currentReader);
            }
        }  
    }
}
