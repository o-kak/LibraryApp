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
    public partial class ChangeReaderForm : Form
    {
        private readonly BookView _bookView;
        private readonly ReaderView _readerView;
        private readonly LoanView _loanView;

        private int currentID;
        private List<BookEventArgs> _borrowedBooks = new List<BookEventArgs>();

        public ChangeReaderForm(ListViewItem item, BookView bookService, ReaderView readerService, LoanView loanService)
        {
            
            _bookView = bookService;
            _readerView = readerService;
            _loanView = loanService;
            InitializeComponent();
            NeedToUpdateNameTextBox.Text = item.SubItems[0].Text;
            NeedToUpdateAdressTextBox.Text = item.SubItems[1].Text;
            currentID = int.Parse(item.SubItems[2].Text);
            LoadBorrowedBooks();
        }

        private void LoadBorrowedBooks()
        {
            ReturnOrBorrowBookCheckedListBox.Items.Clear();
            ReturnOrBorrowBookCheckedListBox.Items.Add("Загрузка книг...");

            // Запрашиваем книги читателя через LoanView
            _loanView.TriggerGetReadersBorrowedBooks(currentID);
        }

        /// <summary>
        /// Кнопка сохранения измененного имени, адреса, состояния книг
        /// </summary>
        private void Savebutton1_Click(object sender, EventArgs e)
        {
            string newName = NeedToUpdateNameTextBox.Text;
            string newAddress = NeedToUpdateAdressTextBox.Text;

            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newAddress))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.",
                              "Ошибка",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            var readerEventArgs = new ReaderEventArgs 
            {
                Id = currentID,
                Name = newName,
                Address = newAddress
            };

            _readerView.TriggerUpdateData(readerEventArgs);
            ProcessBookReturns();


            _loanView.ShowMessage("Информация о читателе успешно обновлена!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ProcessBookReturns()
        {

            for (int i = 0; i < ReturnOrBorrowBookCheckedListBox.Items.Count; i++)
            {
                if (ReturnOrBorrowBookCheckedListBox.Items[i] is BookEventArgs book)
                {
                    bool isChecked = ReturnOrBorrowBookCheckedListBox.GetItemChecked(i);

                    if (!isChecked)
                    {
                        _loanView.TriggerReturnBook(book.Id, currentID);
                    }
                }
            }
        }

    }
}
