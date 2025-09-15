namespace WindowsFormsView
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.AddReaderButton = new System.Windows.Forms.Button();
            this.AddBookButton = new System.Windows.Forms.Button();
            this.ReaderListView = new System.Windows.Forms.ListView();
            this.ReaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReaderAdress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReaderID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReaderBooks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BookListView = new System.Windows.Forms.ListView();
            this.BookName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BookAuther = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BookGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.IsAvailableCheckBox = new System.Windows.Forms.CheckBox();
            this.DeleteReaderButton = new System.Windows.Forms.Button();
            this.DeleteBookButton = new System.Windows.Forms.Button();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.BorrowedBookCheckBox1 = new System.Windows.Forms.CheckBox();
            this.ChangeInfoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddReaderButton
            // 
            this.AddReaderButton.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddReaderButton.Location = new System.Drawing.Point(12, 21);
            this.AddReaderButton.Name = "AddReaderButton";
            this.AddReaderButton.Size = new System.Drawing.Size(250, 86);
            this.AddReaderButton.TabIndex = 0;
            this.AddReaderButton.Text = "Добавить читателя";
            this.AddReaderButton.UseVisualStyleBackColor = true;
            this.AddReaderButton.Click += new System.EventHandler(this.AddReaderButton_Click);
            // 
            // AddBookButton
            // 
            this.AddBookButton.Font = new System.Drawing.Font("Elephant", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddBookButton.Location = new System.Drawing.Point(716, 21);
            this.AddBookButton.Name = "AddBookButton";
            this.AddBookButton.Size = new System.Drawing.Size(250, 86);
            this.AddBookButton.TabIndex = 3;
            this.AddBookButton.Text = "       Добавить        книгу";
            this.AddBookButton.UseVisualStyleBackColor = true;
            this.AddBookButton.Click += new System.EventHandler(this.AddBookButton_Click);
            // 
            // ReaderListView
            // 
            this.ReaderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ReaderName,
            this.ReaderAdress,
            this.ReaderID,
            this.ReaderBooks});
            this.ReaderListView.FullRowSelect = true;
            this.ReaderListView.GridLines = true;
            this.ReaderListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ReaderListView.HideSelection = false;
            this.ReaderListView.LabelEdit = true;
            this.ReaderListView.Location = new System.Drawing.Point(12, 227);
            this.ReaderListView.MultiSelect = false;
            this.ReaderListView.Name = "ReaderListView";
            this.ReaderListView.Size = new System.Drawing.Size(672, 527);
            this.ReaderListView.TabIndex = 4;
            this.ReaderListView.UseCompatibleStateImageBehavior = false;
            this.ReaderListView.View = System.Windows.Forms.View.Details;
            // 
            // ReaderName
            // 
            this.ReaderName.Text = "ФИО";
            this.ReaderName.Width = 130;
            // 
            // ReaderAdress
            // 
            this.ReaderAdress.Text = "Адрес";
            this.ReaderAdress.Width = 100;
            // 
            // ReaderID
            // 
            this.ReaderID.Text = "ID";
            this.ReaderID.Width = 40;
            // 
            // ReaderBooks
            // 
            this.ReaderBooks.Text = "Книги";
            this.ReaderBooks.Width = 436;
            // 
            // BookListView
            // 
            this.BookListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BookName,
            this.BookAuther,
            this.BookGenre});
            this.BookListView.FullRowSelect = true;
            this.BookListView.GridLines = true;
            this.BookListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.BookListView.HideSelection = false;
            this.BookListView.LabelEdit = true;
            this.BookListView.Location = new System.Drawing.Point(716, 227);
            this.BookListView.Name = "BookListView";
            this.BookListView.Size = new System.Drawing.Size(616, 527);
            this.BookListView.TabIndex = 5;
            this.BookListView.UseCompatibleStateImageBehavior = false;
            this.BookListView.View = System.Windows.Forms.View.Details;
            // 
            // BookName
            // 
            this.BookName.Text = "Название";
            this.BookName.Width = 200;
            // 
            // BookAuther
            // 
            this.BookAuther.Text = "Автор";
            this.BookAuther.Width = 104;
            // 
            // BookGenre
            // 
            this.BookGenre.Text = "Жанр";
            this.BookGenre.Width = 143;
            // 
            // IsAvailableCheckBox
            // 
            this.IsAvailableCheckBox.AutoSize = true;
            this.IsAvailableCheckBox.Location = new System.Drawing.Point(1161, 83);
            this.IsAvailableCheckBox.Name = "IsAvailableCheckBox";
            this.IsAvailableCheckBox.Size = new System.Drawing.Size(160, 24);
            this.IsAvailableCheckBox.TabIndex = 6;
            this.IsAvailableCheckBox.Text = "Книга в наличии";
            this.IsAvailableCheckBox.UseVisualStyleBackColor = true;
            this.IsAvailableCheckBox.CheckedChanged += new System.EventHandler(this.IsAvailableCheckBox_CheckedChanged);
            // 
            // DeleteReaderButton
            // 
            this.DeleteReaderButton.Location = new System.Drawing.Point(143, 113);
            this.DeleteReaderButton.Name = "DeleteReaderButton";
            this.DeleteReaderButton.Size = new System.Drawing.Size(119, 86);
            this.DeleteReaderButton.TabIndex = 7;
            this.DeleteReaderButton.Text = "Удалить читателя";
            this.DeleteReaderButton.UseVisualStyleBackColor = true;
            this.DeleteReaderButton.Click += new System.EventHandler(this.DeleteReaderButton_Click);
            // 
            // DeleteBookButton
            // 
            this.DeleteBookButton.Location = new System.Drawing.Point(716, 113);
            this.DeleteBookButton.Name = "DeleteBookButton";
            this.DeleteBookButton.Size = new System.Drawing.Size(250, 86);
            this.DeleteBookButton.TabIndex = 8;
            this.DeleteBookButton.Text = "Удалить книгу";
            this.DeleteBookButton.UseVisualStyleBackColor = true;
            this.DeleteBookButton.Click += new System.EventHandler(this.DeleteBookButton_Click);
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(1161, 145);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(160, 41);
            this.UpdateButton.TabIndex = 9;
            this.UpdateButton.Text = "Весь список";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // BorrowedBookCheckBox1
            // 
            this.BorrowedBookCheckBox1.AutoSize = true;
            this.BorrowedBookCheckBox1.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.BorrowedBookCheckBox1.Location = new System.Drawing.Point(1161, 113);
            this.BorrowedBookCheckBox1.Name = "BorrowedBookCheckBox1";
            this.BorrowedBookCheckBox1.Size = new System.Drawing.Size(182, 24);
            this.BorrowedBookCheckBox1.TabIndex = 10;
            this.BorrowedBookCheckBox1.Text = "Книги не в наличии";
            this.BorrowedBookCheckBox1.UseVisualStyleBackColor = true;
            this.BorrowedBookCheckBox1.CheckedChanged += new System.EventHandler(this.BorrowedBookCheckBox1_CheckedChanged);
            // 
            // ChangeInfoButton
            // 
            this.ChangeInfoButton.Location = new System.Drawing.Point(13, 114);
            this.ChangeInfoButton.Name = "ChangeInfoButton";
            this.ChangeInfoButton.Size = new System.Drawing.Size(112, 85);
            this.ChangeInfoButton.TabIndex = 11;
            this.ChangeInfoButton.Text = "Изменить данные";
            this.ChangeInfoButton.UseVisualStyleBackColor = true;
            this.ChangeInfoButton.Click += new System.EventHandler(this.ChangeInfoButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 794);
            this.Controls.Add(this.ChangeInfoButton);
            this.Controls.Add(this.BorrowedBookCheckBox1);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.DeleteBookButton);
            this.Controls.Add(this.DeleteReaderButton);
            this.Controls.Add(this.IsAvailableCheckBox);
            this.Controls.Add(this.BookListView);
            this.Controls.Add(this.ReaderListView);
            this.Controls.Add(this.AddBookButton);
            this.Controls.Add(this.AddReaderButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddReaderButton;
        private System.Windows.Forms.Button AddBookButton;
        private System.Windows.Forms.ListView ReaderListView;
        private System.Windows.Forms.ListView BookListView;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.CheckBox IsAvailableCheckBox;
        private System.Windows.Forms.Button DeleteReaderButton;
        private System.Windows.Forms.Button DeleteBookButton;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.ColumnHeader ReaderName;
        private System.Windows.Forms.ColumnHeader ReaderAdress;
        private System.Windows.Forms.ColumnHeader ReaderID;
        private System.Windows.Forms.ColumnHeader ReaderBooks;
        private System.Windows.Forms.ColumnHeader BookName;
        private System.Windows.Forms.ColumnHeader BookAuther;
        private System.Windows.Forms.ColumnHeader BookGenre;
        private System.Windows.Forms.CheckBox BorrowedBookCheckBox1;
        private System.Windows.Forms.Button ChangeInfoButton;
    }
}

