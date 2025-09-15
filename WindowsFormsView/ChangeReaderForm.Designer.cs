namespace WindowsFormsView
{
    partial class ChangeReaderForm
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
            this.NeedToUpdateNameTextBox = new System.Windows.Forms.TextBox();
            this.NeedToUpdateAdressTextBox = new System.Windows.Forms.TextBox();
            this.ReturnOrBorrowBookCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NeedToUpdateNameTextBox
            // 
            this.NeedToUpdateNameTextBox.Location = new System.Drawing.Point(72, 113);
            this.NeedToUpdateNameTextBox.Name = "NeedToUpdateNameTextBox";
            this.NeedToUpdateNameTextBox.Size = new System.Drawing.Size(321, 26);
            this.NeedToUpdateNameTextBox.TabIndex = 0;
            // 
            // NeedToUpdateAdressTextBox
            // 
            this.NeedToUpdateAdressTextBox.Location = new System.Drawing.Point(72, 187);
            this.NeedToUpdateAdressTextBox.Name = "NeedToUpdateAdressTextBox";
            this.NeedToUpdateAdressTextBox.Size = new System.Drawing.Size(321, 26);
            this.NeedToUpdateAdressTextBox.TabIndex = 1;
            // 
            // ReturnOrBorrowBookCheckedListBox
            // 
            this.ReturnOrBorrowBookCheckedListBox.FormattingEnabled = true;
            this.ReturnOrBorrowBookCheckedListBox.Location = new System.Drawing.Point(72, 287);
            this.ReturnOrBorrowBookCheckedListBox.Name = "ReturnOrBorrowBookCheckedListBox";
            this.ReturnOrBorrowBookCheckedListBox.Size = new System.Drawing.Size(321, 188);
            this.ReturnOrBorrowBookCheckedListBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(44, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 37);
            this.label1.TabIndex = 3;
            this.label1.Text = "Изменить данные читателя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "ФИО";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Адрес";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(150, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Вернуть|Взять Книги";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(146, 578);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 54);
            this.button1.TabIndex = 9;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Savebutton1_Click);
            // 
            // ChangeReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 644);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReturnOrBorrowBookCheckedListBox);
            this.Controls.Add(this.NeedToUpdateAdressTextBox);
            this.Controls.Add(this.NeedToUpdateNameTextBox);
            this.Name = "ChangeReaderForm";
            this.Text = "ChangeReaderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NeedToUpdateNameTextBox;
        private System.Windows.Forms.TextBox NeedToUpdateAdressTextBox;
        private System.Windows.Forms.CheckedListBox ReturnOrBorrowBookCheckedListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}