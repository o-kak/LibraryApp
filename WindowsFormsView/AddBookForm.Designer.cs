namespace WindowsFormsView
{
    partial class AddBookForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBookForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TitleText = new System.Windows.Forms.TextBox();
            this.AuthorText = new System.Windows.Forms.TextBox();
            this.GenreText = new System.Windows.Forms.TextBox();
            this.GenreLabel = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.TitleBookLabel = new System.Windows.Forms.Label();
            this.SaveBookButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(111, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(285, 203);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TitleText
            // 
            this.TitleText.Location = new System.Drawing.Point(42, 241);
            this.TitleText.Name = "TitleText";
            this.TitleText.Size = new System.Drawing.Size(388, 26);
            this.TitleText.TabIndex = 1;
            // 
            // AuthorText
            // 
            this.AuthorText.Location = new System.Drawing.Point(42, 349);
            this.AuthorText.Name = "AuthorText";
            this.AuthorText.Size = new System.Drawing.Size(388, 26);
            this.AuthorText.TabIndex = 2;
            // 
            // GenreText
            // 
            this.GenreText.Location = new System.Drawing.Point(42, 462);
            this.GenreText.Name = "GenreText";
            this.GenreText.Size = new System.Drawing.Size(388, 26);
            this.GenreText.TabIndex = 3;
            // 
            // GenreLabel
            // 
            this.GenreLabel.AutoSize = true;
            this.GenreLabel.Location = new System.Drawing.Point(218, 439);
            this.GenreLabel.Name = "GenreLabel";
            this.GenreLabel.Size = new System.Drawing.Size(49, 20);
            this.GenreLabel.TabIndex = 4;
            this.GenreLabel.Text = "Жанр";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(211, 326);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(56, 20);
            this.AuthorLabel.TabIndex = 5;
            this.AuthorLabel.Text = "Автор";
            // 
            // TitleBookLabel
            // 
            this.TitleBookLabel.AutoSize = true;
            this.TitleBookLabel.Location = new System.Drawing.Point(201, 218);
            this.TitleBookLabel.Name = "TitleBookLabel";
            this.TitleBookLabel.Size = new System.Drawing.Size(83, 20);
            this.TitleBookLabel.TabIndex = 6;
            this.TitleBookLabel.Text = "Название";
            // 
            // SaveBookButton
            // 
            this.SaveBookButton.Location = new System.Drawing.Point(167, 554);
            this.SaveBookButton.Name = "SaveBookButton";
            this.SaveBookButton.Size = new System.Drawing.Size(144, 60);
            this.SaveBookButton.TabIndex = 7;
            this.SaveBookButton.Text = "Сохранить";
            this.SaveBookButton.UseVisualStyleBackColor = true;
            this.SaveBookButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // AddBookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 644);
            this.Controls.Add(this.SaveBookButton);
            this.Controls.Add(this.TitleBookLabel);
            this.Controls.Add(this.AuthorLabel);
            this.Controls.Add(this.GenreLabel);
            this.Controls.Add(this.GenreText);
            this.Controls.Add(this.AuthorText);
            this.Controls.Add(this.TitleText);
            this.Controls.Add(this.pictureBox1);
            this.Name = "AddBookForm";
            this.Text = "AddBookForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox TitleText;
        private System.Windows.Forms.TextBox AuthorText;
        private System.Windows.Forms.TextBox GenreText;
        private System.Windows.Forms.Label GenreLabel;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.Label TitleBookLabel;
        private System.Windows.Forms.Button SaveBookButton;
    }
}