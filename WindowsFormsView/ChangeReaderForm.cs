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
        private List<BookEventArgs> _allBooks = new List<BookEventArgs>();
        private List<BookEventArgs> _borrowedBooks = new List<BookEventArgs>();

        public static ChangeReaderForm CurrentInstance { get; private set; }

        public ChangeReaderForm(ListViewItem item, BookView bookService, ReaderView readerService, LoanView loanService)
        {
            
            _bookView = bookService;
            _readerView = readerService;
            _loanView = loanService;

            InitializeComponent();

            NeedToUpdateNameTextBox.Text = item.SubItems[0].Text;
            NeedToUpdateAdressTextBox.Text = item.SubItems[1].Text;
            currentID = int.Parse(item.SubItems[2].Text);

            CurrentInstance = this;

            _loanView.TriggerGetAvailableBooks();
            _loanView.TriggerGetReadersBorrowedBooks(currentID);
        }

        /// <summary>
        /// Метод обновляющий список доступных книг, для отображения в ЛистЧекБокс для взятия книг
        /// </summary>
        public void UpdateAllBooks(IEnumerable<EventArgs> books) 
        {
            _allBooks = books.OfType<BookEventArgs>().ToList();
            UpdateBorrowBooksListBox();
        }

        /// <summary>
        /// Метод обновляющий список книг взятых пользователем, для отображения в ЛистЧекБокс для возврата книг
        /// </summary>
        public void UpdateReaderBooks(IEnumerable<EventArgs> books)
        {
            _borrowedBooks = books.OfType<BookEventArgs>().ToList();
            UpdateReturnBooksListBox();
        }

        /// <summary>
        /// Метод добавляющий доступные книги в ЛистЧекБокс для взятия книг 
        /// </summary>
        private void UpdateBorrowBooksListBox() 
        {
            if (BorrowBookCheckedListBox.InvokeRequired)
            {
                BorrowBookCheckedListBox.Invoke(new Action(() => UpdateBorrowBooksListBox()));
                return;
            }

            BorrowBookCheckedListBox.Items.Clear();
            foreach (var book in _allBooks) 
            {
                if (!_borrowedBooks.Any(b => b.Id == book.Id))
                {
                    BorrowBookCheckedListBox.Items.Add(book.Title, false);
                }
            }

            if (_allBooks.Count == 0)
            {
                BorrowBookCheckedListBox.Items.Add("Нет книг в библиотеке");
                ReturnBookscheckedListBox1.Enabled = false;
            }
            else 
            {
                ReturnBookscheckedListBox1.Enabled = true;
            }
        }

        /// <summary>
        /// Метод добавляющий книги пользователя в ЛистЧекБокс для возварата книг 
        /// </summary>
        private void UpdateReturnBooksListBox()
        {
            if (ReturnBookscheckedListBox1.InvokeRequired)
            {
                ReturnBookscheckedListBox1.Invoke(new Action(() => UpdateReturnBooksListBox()));
                return;
            }

            ReturnBookscheckedListBox1.Items.Clear();
            foreach (var book in _borrowedBooks)
            {
                ReturnBookscheckedListBox1.Items.Add(book.Title, false);
            }

            if (ReturnBookscheckedListBox1.Items.Count == 0)
            {
                ReturnBookscheckedListBox1.Items.Add("Нет книг для возврата");
                ReturnBookscheckedListBox1.Enabled = false;
            }
            else
            {
                ReturnBookscheckedListBox1.Enabled = true;
            }
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

            ProcessBookOperations();
            _readerView.TriggerUpdateData(readerEventArgs);
            
            _loanView.ShowMessage("Информация о читателе успешно обновлена!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Метод, который для каждого ЛистЧекБокса либо возвращает книги, либо дает книги читателю
        /// </summary>
        private void ProcessBookOperations() 
        {

            for (int i = 0; i < ReturnBookscheckedListBox1.Items.Count; i++)
            {
                if (ReturnBookscheckedListBox1.Items[i].ToString() == "Нет книг для возврата")
                    continue;

                if (ReturnBookscheckedListBox1.GetItemChecked(i))
                {
                    string bookTitle = ReturnBookscheckedListBox1.Items[i].ToString();
                    var book = _borrowedBooks.FirstOrDefault(b => b.Title == bookTitle);

                    if (book != null)
                    {
                        _loanView.TriggerReturnBook(book.Id, currentID);
                    }
                }
            }

            for (int i = 0; i < BorrowBookCheckedListBox.Items.Count; i++)
            {
                if (BorrowBookCheckedListBox.Items[i].ToString() == "Нет книг в библиотеке")
                    continue;

                if (BorrowBookCheckedListBox.GetItemChecked(i))
                {
                    string bookTitle = BorrowBookCheckedListBox.Items[i].ToString();
                    var book = _allBooks.FirstOrDefault(b => b.Title == bookTitle);

                    if (book != null)
                    {
                        _loanView.TriggerGiveBook(book.Id, currentID);
                    }
                }
            }
        }
    }
}
