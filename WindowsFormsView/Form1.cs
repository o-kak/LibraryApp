using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;
using Shared;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsView
{
    public partial class Form1: Form
    {
        private BookView _bookView;
        private ReaderView _readerView;
        private LoanView _loanView;

        public Form1()
        {
            InitializeComponent();
            InitializeViews();
            
        }
        public void InitializeViews() 
        {
            _bookView = new BookView(this);
            _readerView = new ReaderView(this);
            _loanView = new LoanView(this);
        }

        private void LoadAuthorsAndGenres() 
        {
            var books= bookService.GetAllBooks().ToList();
            var authors = books.Select(x => x.Author).Distinct().ToList();
            AuthorComboBox2.DataSource = authors;

            var genres = books.Select(x => x.Genre).Distinct().ToList();
            GenreComboBox1.DataSource = genres;
        }


        //РАБОТА С ТАБЛИЦАМИ

        //-------------------------------------------------------------------------------------------------ЧИТАТЕЛИ---------------------------------------------------------------------------- 

        /// <summary>
        /// Обновление таблицы читателей
        /// </summary>
        /// <param name="readers">список читателей</param>
        public void RedrawReader(IEnumerable<EventArgs> readerEventArgs)
        {
            var readerEvents = readerEventArgs.OfType<ReaderEventArgs>();

            if (ReaderListView.InvokeRequired)
            {
                ReaderListView.Invoke(new Action(() => RedrawReader(readerEventArgs)));
                return;
            }

            ReaderListView.BeginUpdate();
            ReaderListView.Items.Clear();

            foreach (var readerEvent in readerEvents)
            {
                ListViewItem item = new ListViewItem(readerEvent.Name);
                item.SubItems.Add(readerEvent.Address);
                item.SubItems.Add(readerEvent.Id.ToString());

                item.Tag = readerEvent.Id;

                ReaderListView.Items.Add(item);
            }
            ReaderListView.EndUpdate();
        }

        /// <summary>
        /// Кнопка, для удаления выделенного читателя из таблицы
        /// </summary>
        private void DeleteReaderButton_Click(object sender, EventArgs e)
        {
            if (ReaderListView.SelectedItems.Count > 0)
            {
                var selectedItem = ReaderListView.SelectedItems[0];

                if (selectedItem.Tag is int readerId)
                {
                    var result = MessageBox.Show("Вы уверены, что хотите удалить читателя?",
                                                "Подтверждение удаления",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        _readerView.TriggerDeleteData(readerId);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось распознать ID читателя из выбранной строки.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
            }
        }


        //-------------------------------------------------------------------------------------КНИГИ----------------------------------------------------------------------------------------

        /// <summary>
        /// Обновление таблицы с книгами
        /// </summary>
        /// <param name="books">список книг</param>
        public void RedrawBook(IEnumerable<EventArgs> bookEventArgs)
        {
            var bookEvents = bookEventArgs.OfType<BookEventArgs>();
            if (BookListView.InvokeRequired)
            {
                BookListView.Invoke(new Action(() => RedrawBook(bookEventArgs)));
                return;
            }
            BookListView.BeginUpdate();
            BookListView.Items.Clear();

            foreach (var bookEvent in bookEvents)
            {
                ListViewItem b_item = new ListViewItem(bookEvent.Title);
                b_item.SubItems.Add(bookEvent.Author);
                b_item.SubItems.Add(bookEvent.Genre);
                string readerName = "Нет читателя";
                if (bookEvent.ReaderId.HasValue && bookEvent.ReaderId.Value > 0)
                {
                    readerName = $"Читатель ID: {bookEvent.ReaderId.Value}";
                }

                b_item.SubItems.Add(readerName);
                BookListView.Items.Add(b_item);
            }
            BookListView.EndUpdate();
        }

        /// <summary>
        /// Кнопка, для удаления выделенной книги из таблицы
        /// </summary>
        private void DeleteBookButton_Click(object sender, EventArgs e)
        {
            if (BookListView.SelectedItems.Count > 0)
            {
                var selectedItem = BookListView.SelectedItems[0];
                if (selectedItem.Tag is int bookId)
                {
                    var result = MessageBox.Show("Вы уверены, что хотите удалить книгу?",
                                                "Подтверждение удаления",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        _bookView.TriggerDeleteData(bookId);
                    }
                    else
                    {
                        MessageBox.Show("Книга не найдена.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
            }

        }

        /// <summary>
        /// Кнопка, для обновления таблицы с книгами
        /// </summary>
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            BorrowedBookCheckBox1.Checked = false;
            IsAvailableCheckBox.Checked = false;
            AuthorComboBox2.SelectedIndex = -1;
            GenreComboBox1.SelectedIndex = -1;
            _bookView.TriggerStartup();
        }



        /// <summary>
        /// показать доступные книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsAvailableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAvailableCheckBox.Checked)
            {
                BorrowedBookCheckBox1.Checked = false;
                _bookView.TriggerGetAvailableBooks();

            }
        }

        /// <summary>
        /// показать недоступные книги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BorrowedBookCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (BorrowedBookCheckBox1.Checked)
            {
                IsAvailableCheckBox.Checked = false;
                _bookView.TriggerGetBorrowedBooks();
            }

        }

        /// <summary>
        /// при выборе жанра в комбобокс происходит фильтрация книг по жанру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenreComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string genre = GenreComboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(genre))
            {
                GenreComboBox1.Text = "Жанры";
                return;
            }

            _bookView.TriggerFilterByGenre(genre);
        }

        /// <summary>
        /// при выборе автора в комбобокс происходит фильтрация книг по автору
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuthorComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string author = AuthorComboBox2.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(author))
            {
                AuthorComboBox2.Text = "Авторы";
                return;
            }
            _bookView.TriggerFilterByAuthor(author);
        }


        //-------------------------------------------------------------------------КНОПКИ ДЛЯ ПЕРЕХОДА НА ДРУГИЕ ОКНА-----------------------------------------------------


        /// <summary>
        /// Кнопка, для открытия окна для создания нового читателя
        /// </summary>
        private void AddReaderButton_Click(object sender, EventArgs e)
        {
            using (AddReaderForm arf = new AddReaderForm(_bookView, _readerView, _loanView))
            {
                arf.ShowDialog();
            }
        }

        /// <summary>
        /// Кнопка, для открытия окна для создания новой книги
        /// </summary>
        private void AddBookButton_Click(object sender, EventArgs e)
        {
            using (AddBookForm abf = new AddBookForm(_bookView))
            {
                abf.ShowDialog();
            }
        }
        
        /// <summary>
        /// Кнопка для перехода к новому окну, для изменения данных выбранного пользователя
        /// </summary>
        private void ChangeInfoButton_Click(object sender, EventArgs e)
        {
            if (ReaderListView.SelectedItems.Count >0)
            {
                ListViewItem selectedItem = ReaderListView.SelectedItems[0];
                using (ChangeReaderForm crf = new ChangeReaderForm(selectedItem, _bookView, _readerView, _loanView)) 
                {
                    crf.ShowDialog();
                }
            }
        }

    }
}
