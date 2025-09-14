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
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private LibraryManager _libraryManager;


        private void AddReaderButton_Click(object sender, EventArgs e)
        {
            using (AddReaderForm arf = new AddReaderForm()) 
            {
                arf.ShowDialog();
            }
        }

        private void AddBookButton_Click(object sender, EventArgs e)
        {

        }

        private void DeleteReaderButton_Click(object sender, EventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {

        }
        public void UpdateReaderListView(Reader reader)
        {
            ListViewItem item = new ListViewItem(reader.Name);
            item.SubItems.Add(reader.Address);
            item.SubItems.Add(reader.ID.ToString());

            string borrowedBooks = String.Join(",", reader.BooksBorrowed.Select(b => b.Title));

            item.SubItems.Add(borrowedBooks);           //не отображаются книги:(

            ReaderListView.Items.Add(item);
        }
    }
}
