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
    public partial class AddReaderForm : Form
    {

        private readonly BookView _bookView;
        private readonly ReaderView _readerView;
        private readonly LoanView _loanView;

        private List<BookEventArgs> _availableBooks = new List<BookEventArgs>();
        private List<int> _selectedBooksIds = new List<int>();

        public AddReaderForm(BookView bookView, ReaderView readerView, LoanView loanView)
        {
            _bookView = bookView;
            _readerView = readerView;
            _loanView = loanView;

            InitializeComponent();
            LoadAvailableBooks();
        }

        private void LoadAvailableBooks() 
        {
            _availableBooks.Clear();
            _loanView.TriggerGetAvailableBooks();
            FillBorrowedBooksCheckedListBox();
        }

        private void FillBorrowedBooksCheckedListBox()
        {
            BorrowedBooksCheckedListBox.Items.Clear();
            foreach (var book in _availableBooks)
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

            _selectedBooksIds.Clear();
            for (int i = 0; i < BorrowedBooksCheckedListBox.Items.Count; i++)
            {
                if (BorrowedBooksCheckedListBox.GetItemChecked(i))
                {
                    if (BorrowedBooksCheckedListBox.Items[i].Tag is int bookId)
                    {
                        _selectedBooksIds.Add(bookId);
                    }
                }
            }



        }
    }
}
