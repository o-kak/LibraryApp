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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsView
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();

            if (!libraryManager.Books.Any())
            {
                libraryManager.AddBook("1984", "George Orwell", "Dystopian");
                libraryManager.AddBook("Brave New World", "Aldous Huxley", "Science Fiction");
                libraryManager.AddBook("Fahrenheit 451", "Ray Bradbury", "Dystopian");
                UpdateBooksListView(libraryManager.Books);
            }
        }
        LibraryManager libraryManager = new LibraryManager();

        /// <summary>
        /// Кнопка, для открытия окна для создания нового читателя
        /// </summary>
        private void AddReaderButton_Click(object sender, EventArgs e)
        {
            using (AddReaderForm arf = new AddReaderForm(libraryManager)) 
            {
                arf.ShowDialog();
            }
        }

        /// <summary>
        /// Кнопка, для открытия окна для создания новой книги
        /// </summary>
        private void AddBookButton_Click(object sender, EventArgs e)
        {
            using (AddBookForm abf = new AddBookForm(libraryManager))
            {
                abf.ShowDialog();
            }
        }

        /// <summary>
        /// Кнопка, для обновления таблицы с книгами
        /// </summary>
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            BorrowedBookCheckBox1.Checked = false;
            IsAvailableCheckBox.Checked = false;
            UpdateBooksListView(libraryManager.Books);
        }

        /// <summary>
        /// Кнопка, для удаления выделенного читателя из таблицы
        /// </summary>
        private void DeleteReaderButton_Click(object sender, EventArgs e)
        {
            if (ReaderListView.SelectedItems.Count > 0)
            {
                var selectedItem = ReaderListView.SelectedItems[0];
                int readerId = int.Parse(selectedItem.SubItems[2].Text); 

                var readerToDelete = libraryManager.Readers.FirstOrDefault(x => x.ID == readerId);

                if (readerToDelete != null)
                {
                    libraryManager.DeleteReader(readerToDelete);
                    UpdateReaderListView(libraryManager.Readers); 
                    ReaderListView.Refresh();
                }
                else
                {
                    MessageBox.Show("Читатель не найден.");
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
                item.SubItems.Add(reader.ID.ToString());

                string borrowedBooks = reader.BooksBorrowed != null && reader.BooksBorrowed.Any()
                    ? String.Join(", ", reader.BooksBorrowed.Select(b => b.Title))
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
                BookListView.Items.Add(b_item);
            }
            BookListView.EndUpdate();
        }

        private void IsAvailableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAvailableCheckBox.Checked)
            {
                BorrowedBookCheckBox1.Checked = false;
                UpdateBooksListView(libraryManager.GetAvailableBooks());
                
            }
        }

        private void BorrowedBookCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (BorrowedBookCheckBox1.Checked) 
            {
                IsAvailableCheckBox.Checked = false;
                UpdateBooksListView(libraryManager.GetBorrowedBooks());
            }

        }

        /// <summary>
        /// Кнопка, для удаления выделенной книги из таблицы
        /// </summary>
        private void DeleteBookButton_Click(object sender, EventArgs e)
        {
            if (BookListView.SelectedItems.Count > 0)
            {
                var selectedItem = BookListView.SelectedItems[0];
                string bookTitle = selectedItem.SubItems[0].Text;

                var bookToDelete = libraryManager.Books.FirstOrDefault(x => x.Title == bookTitle);

                if (bookToDelete != null)
                {
                    libraryManager.DeleteBook(bookToDelete);
                    UpdateBooksListView(libraryManager.Books);
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
                using (ChangeReaderForm crf = new ChangeReaderForm(selectedItem, libraryManager)) 
                {
                    crf.ShowDialog();
                }
            }
        }
    }
}
