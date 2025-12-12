using Ninject;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsView
{
    public partial class Form1: Form
    {
        public BookView _bookView;
        public ReaderView _readerView;
        public LoanView _loanView;

        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;
        private ToolStripProgressBar _progressBar;

        public Form1()
        {
            InitializeComponent();
            InitializeStatusBar();
            InitializeViews();
            StartDataLoading();
            
        }
        private void InitializeStatusBar()
        {
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("Инициализация...");
            _progressBar = new ToolStripProgressBar();
            _progressBar.Style = ProgressBarStyle.Marquee; // Бегущая строка

            _statusStrip.Items.Add(_statusLabel);
            _statusStrip.Items.Add(_progressBar);

            this.Controls.Add(_statusStrip);
        }
        public void InitializeViews() 
        {
            _bookView = new BookView(this);
            _readerView = new ReaderView(this);
            _loanView = new LoanView(this);
        }

        private void StartDataLoading()
        {
            UpdateStatus("Начало загрузки данных...");
            Task.Run(() =>
            {
                UpdateStatus("Загрузка книг...");
                _bookView.Start();


                Thread.Sleep(100);
                UpdateStatus("Загрузка читателей...");
                _readerView.Start();


                UpdateStatus("Данные загружены");
                Thread.Sleep(500); 
                UpdateStatus("Готово");

                this.Invoke(new Action(() => _progressBar.Visible = false));
            });
        }

        private void UpdateStatus(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateStatus(message)));
            }
            else
            {
                _statusLabel.Text = message;
                Application.DoEvents(); // Принудительно обновляем UI
                Console.WriteLine($"[STATUS] {message}"); // Также пишем в консоль
            }
        }


        //РАБОТА С ТАБЛИЦАМИ

        //-------------------------------------------------------------------------------------------------ЧИТАТЕЛИ---------------------------------------------------------------------------- 

        /// <summary>
        /// Обновление таблицы читателей
        /// </summary>
        /// <param name="readers">список читателей</param>
        public void RedrawReader(IEnumerable<EventArgs> readerEventArgs)
        {
            ReaderListView.BeginUpdate();
            var readerEvents = readerEventArgs.OfType<ReaderEventArgs>();

            if (ReaderListView.InvokeRequired)
            {
                ReaderListView.Invoke(new Action(() => RedrawReader(readerEventArgs)));
                return;
            }

            ReaderListView.Items.Clear();

            foreach (var readerEvent in readerEvents)
            {
                ListViewItem item = new ListViewItem(readerEvent.Name);
                item.SubItems.Add(readerEvent.Address);
                item.SubItems.Add(readerEvent.Id.ToString());
                item.SubItems.Add("");

                item.Tag = readerEvent.Id;

                ReaderListView.Items.Add(item);
                _readerView.TriggerGetBorrowedBooks(readerEvent.Id);
            }
            ReaderListView.EndUpdate();
        }

        public void ShowBorrowedBooksDialog(IEnumerable<BookEventArgs> books)
        {
            if (ReaderListView.InvokeRequired)
            {
                ReaderListView.Invoke(new Action(() => ShowBorrowedBooksDialog(books)));
                return;
            }
            foreach (var book in books) 
            {
                    Console.WriteLine($"{book.Title} — {book.Author} - {book.ReaderId}");
               
            }
            var firstBookWithReaderId = books.FirstOrDefault(b => b.ReaderId.HasValue);

            if (firstBookWithReaderId == null)
            {
                return;
            }

            int readerId = firstBookWithReaderId.ReaderId.Value;

            if (books != null && books.Any())
            {
                var bookTitles = books.Select(b => b.Title).ToList();
                string booksText;
                booksText = string.Join(", ", bookTitles);
                foreach (ListViewItem item in ReaderListView.Items)
                {
                    
                    if (item.Tag is int id && id == readerId)
                    {
                        item.SubItems[3].Text = booksText;

                        break;
                    }
                }
            }
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
            BookListView.BeginUpdate();
            var bookEvents = bookEventArgs.OfType<BookEventArgs>();
            if (BookListView.InvokeRequired)
            {
                BookListView.Invoke(new Action(() => RedrawBook(bookEventArgs)));
                return;
            }
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
                    _bookView.TriggerDeleteData(bookId);
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
