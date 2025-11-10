using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using BusinessLogic;
using Ninject;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsView
{
    public partial class Form1: Form
    {
        private BookService bookService;
        private ReaderService readerService; 
        private BookAuthorFilter bookAuthorFilter;
        private BookGenreFilter bookGenreFilter;
        private LoanService loanService;
        public Form1(BookAuthorFilter authorFilter, BookGenreFilter genreFilter, BookService bookService, LoanService loanService, ReaderService readerService)
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());

            this.bookAuthorFilter = authorFilter;
            this.bookGenreFilter = genreFilter;
            this.bookService = bookService;
            this.loanService = loanService;
            this.readerService = readerService;

            List<Book> books = bookService.GetAllBooks().ToList();
            List<Reader> readers = readerService.GetAllReaders().ToList();
            UpdateBooksListView(books);
            UpdateReaderListView(readers);
            LoadAuthorsAndGenres();

        }

        private void LoadAuthorsAndGenres() 
        {
            var books= bookService.GetAllBooks().ToList();
            var authors = books.Select(x => x.Author).Distinct().ToList();
            AuthorComboBox2.DataSource = authors;

            var genres = books.Select(x => x.Genre).Distinct().ToList();
            GenreComboBox1.DataSource = genres;
        }


        /// <summary>
        /// Кнопка, для открытия окна для создания нового читателя
        /// </summary>
        private void AddReaderButton_Click(object sender, EventArgs e)
        {
            using (AddReaderForm arf = new AddReaderForm(bookService, readerService, loanService)) 
            {
                arf.ShowDialog();
            }
        }

        /// <summary>
        /// Кнопка, для открытия окна для создания новой книги
        /// </summary>
        private void AddBookButton_Click(object sender, EventArgs e)
        {
            using (AddBookForm abf = new AddBookForm(bookService))
            {
                abf.ShowDialog();
            }
        }

        /// <summary>
        /// Кнопка, для обновления таблицы с книгами
        /// </summary>
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            var books = bookService.GetAllBooks().ToList();
            BorrowedBookCheckBox1.Checked = false;
            IsAvailableCheckBox.Checked = false;
            AuthorComboBox2.SelectedIndex = -1;
            GenreComboBox1.SelectedIndex = -1;
            UpdateBooksListView(books);
        }

        /// <summary>
        /// Кнопка, для удаления выделенного читателя из таблицы
        /// </summary>
        private void DeleteReaderButton_Click(object sender, EventArgs e)
        {
            if (ReaderListView.SelectedItems.Count > 0)
            {
                var selectedItem = ReaderListView.SelectedItems[0];

                if (int.TryParse(selectedItem.SubItems[2].Text, out int readerId))
                {
                    readerService.DeleteReader(readerId);
                    var updatedReaders = readerService.GetAllReaders().ToList();
                    UpdateReaderListView(updatedReaders);
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

        /// <summary>
        /// Обновление таблицы читателей
        /// </summary>
        /// <param name="readers">список читателей</param>
        public void UpdateReaderListView(IEnumerable<Reader> readers)
        {
            ReaderListView.BeginUpdate(); 
            ReaderListView.Items.Clear(); 

            foreach (var reader in readers)
            {
                ListViewItem item = new ListViewItem(reader.Name);
                item.SubItems.Add(reader.Address);
                item.SubItems.Add(reader.Id.ToString());

                string borrowedBooks = loanService.GetReadersBorrowedBooks(reader.Id) != null && loanService.GetReadersBorrowedBooks(reader.Id).Any()
                    ? String.Join(", ", loanService.GetReadersBorrowedBooks(reader.Id).Select(b => b.Title))
                    : "Нет заимствованных книг";

                item.SubItems.Add(borrowedBooks);

                ReaderListView.Items.Add(item);
            }
            ReaderListView.EndUpdate(); 
        }

        /// <summary>
        /// Обновление таблицы с книгами
        /// </summary>
        /// <param name="books">список книг</param>
        public void UpdateBooksListView(IEnumerable<Book> books) 
        {
            BookListView.BeginUpdate();
            BookListView.Items.Clear();
            foreach (Book book in books) 
            {
                ListViewItem b_item = new ListViewItem(book.Title);
                b_item.SubItems.Add(book.Author);
                b_item.SubItems.Add(book.Genre);
                string readerName = "Нет читателя"; 
                if (book.ReaderId.HasValue && book.ReaderId.Value > 0) 
                {
                    Reader reader = readerService.GetReader(book.ReaderId.Value);
                    if (reader != null)
                    {
                        readerName = reader.Name; 
                    }
                }

                // Добавляем имя читателя в новый столбец
                b_item.SubItems.Add(readerName);
                BookListView.Items.Add(b_item);
            }
            BookListView.EndUpdate();
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
                UpdateBooksListView(loanService.GetAvailableBooks());
                
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
                UpdateBooksListView(loanService.GetBorrowedBooks());
            }

        }

        /// <summary>
        /// Кнопка, для удаления выделенной книги из таблицы
        /// </summary>
        private void DeleteBookButton_Click(object sender, EventArgs e)
        {
            var books = bookService.GetAllBooks().ToList();
            if (BookListView.SelectedItems.Count > 0)
            {
                var selectedItem = BookListView.SelectedItems[0];
                string bookTitle = selectedItem.SubItems[0].Text;

                var bookToDelete = books.FirstOrDefault(x => x.Title == bookTitle);

                if (bookToDelete != null)
                {
                    bookService.DeleteBook(bookToDelete.Id);
                    var updatedBooks = bookService.GetAllBooks().ToList();
                    UpdateBooksListView(updatedBooks);
                    BookListView.Refresh();
                }
                else
                {
                    MessageBox.Show("Книга не найдена.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
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
                using (ChangeReaderForm crf = new ChangeReaderForm(selectedItem, bookService, readerService, loanService)) 
                {
                    crf.ShowDialog();
                }
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

            UpdateBooksListView(bookGenreFilter.Filter(genre));
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
            UpdateBooksListView(bookAuthorFilter.Filter(author));
        }
    }
}
