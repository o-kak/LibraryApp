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
    public partial class ChangeReaderForm : Form
    {
        

        private int currentID;
        public ChangeReaderForm(ListViewItem item, IBookService bookService, IReaderService readerService, ILoan loanService)
        {
            this.bookService = bookService;
            this.readerService = readerService;
            this.loanService = loanService;
            InitializeComponent();
            NeedToUpdateNameTextBox.Text = item.SubItems[0].Text;
            NeedToUpdateAdressTextBox.Text = item.SubItems[1].Text;
            currentID = int.Parse(item.SubItems[2].Text);
            LoadInfo(item);
        }

        /// <summary>
        /// Перемещение списка книг из таблицы из Главной формы в ListCheckBox этой формы. Если в списке из таблицы были книги, то они автоматически отмечаются.
        /// </summary>
        /// <param name="item">выбранная строка из таблицы читателей</param>
        private void LoadInfo(ListViewItem item) 
        {
            var books = bookService.GetAllBooks().ToList();
            string selectedBooks = item.SubItems[3].Text;
            ReturnOrBorrowBookCheckedListBox.Items.Clear();
            if (selectedBooks == "Нет заимствованных книг")
            {
                foreach (var book in books)
                {
                    ReturnOrBorrowBookCheckedListBox.Items.Add(book, false); 
                }
            }
            else 
            {
                var selectedBooksList = selectedBooks.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(b => b.Trim())
                                          .ToList();

                foreach (var book in books)
                {
                    bool isChecked = selectedBooksList.Contains(book.Title);
                    ReturnOrBorrowBookCheckedListBox.Items.Add(book, isChecked);
                }
            }
        }

        /// <summary>
        /// Кнопка сохранения измененного имени, адреса, состояния книг
        /// </summary>
        private void Savebutton1_Click(object sender, EventArgs e)
        {
            List<Reader> readers = readerService.GetAllReaders().ToList();
            var currentReader = readers.FirstOrDefault(r => r.Id == currentID);
            if (currentReader == null)
            {
                MessageBox.Show("Читатель не найден!");
                return;
            }

            currentReader.UpdateName(NeedToUpdateNameTextBox.Text);
            currentReader.UpdateAddress(NeedToUpdateAdressTextBox.Text);

            foreach (var item in ReturnOrBorrowBookCheckedListBox.Items)
            {
                var book = item as Book;
                if (book != null)
                {
                    bool isChecked = ReturnOrBorrowBookCheckedListBox.GetItemChecked(ReturnOrBorrowBookCheckedListBox.Items.IndexOf(item));

                    if (!isChecked && loanService.GetReadersBorrowedBooks(currentReader.Id).Contains(book))
                    {
                        try
                        {
                            loanService.ReturnBook(book.Id, currentReader.Id);
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    else if (isChecked && !loanService.GetReadersBorrowedBooks(currentReader.Id).Contains(book))
                    {
                        try
                        {
                            loanService.GiveBook(book.Id, currentReader.Id);
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }

            MessageBox.Show("Информация о читателе успешно обновлена!");
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.UpdateReaderListView(readers);
            }

            this.Close();
        }        
    }
}
