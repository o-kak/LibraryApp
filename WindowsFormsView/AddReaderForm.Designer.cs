

namespace WindowsFormsView
{
    partial class AddReaderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddReaderForm));
            this.ReaderNamTextBox = new System.Windows.Forms.TextBox();
            this.ReaderAdressTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SaveReaderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ReaderNamTextBox
            // 
            this.ReaderNamTextBox.Location = new System.Drawing.Point(125, 317);
            this.ReaderNamTextBox.Name = "ReaderNamTextBox";
            this.ReaderNamTextBox.Size = new System.Drawing.Size(268, 26);
            this.ReaderNamTextBox.TabIndex = 0;
            // 
            // ReaderAdressTextBox
            // 
            this.ReaderAdressTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.ReaderAdressTextBox.Location = new System.Drawing.Point(125, 393);
            this.ReaderAdressTextBox.Name = "ReaderAdressTextBox";
            this.ReaderAdressTextBox.Size = new System.Drawing.Size(268, 26);
            this.ReaderAdressTextBox.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(125, 60);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(268, 215);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // SaveReaderButton
            // 
            this.SaveReaderButton.Location = new System.Drawing.Point(164, 504);
            this.SaveReaderButton.Name = "SaveReaderButton";
            this.SaveReaderButton.Size = new System.Drawing.Size(163, 67);
            this.SaveReaderButton.TabIndex = 5;
            this.SaveReaderButton.Text = "Сохранить";
            this.SaveReaderButton.UseVisualStyleBackColor = true;
            this.SaveReaderButton.Click += new System.EventHandler(this.SaveReaderButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 317);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "ФИО";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 393);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Адрес";
            // 
            // AddReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 644);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveReaderButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ReaderAdressTextBox);
            this.Controls.Add(this.ReaderNamTextBox);
            this.Name = "AddReaderForm";
            this.Text = "AddReaderForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ReaderNamTextBox;
        private System.Windows.Forms.TextBox ReaderAdressTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button SaveReaderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}