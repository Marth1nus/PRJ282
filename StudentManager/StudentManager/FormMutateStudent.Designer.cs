
namespace StudentManager
{
    partial class FormMutateStudent
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxStudentNumber = new System.Windows.Forms.TextBox();
            this.textBoxStudentAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxStudentName = new System.Windows.Forms.TextBox();
            this.textBoxStudentPhone = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxStudentBirthdate = new System.Windows.Forms.TextBox();
            this.textBoxStudentGender = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridViewModuleCodes = new System.Windows.Forms.DataGridView();
            this.Module_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Module_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Module_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxStudentImage = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModuleCodes)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 450);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Student";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxStudentNumber);
            this.panel1.Controls.Add(this.textBoxStudentAddress);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxStudentImage);
            this.panel1.Controls.Add(this.textBoxStudentName);
            this.panel1.Controls.Add(this.textBoxStudentPhone);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBoxStudentBirthdate);
            this.panel1.Controls.Add(this.textBoxStudentGender);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 215);
            this.panel1.TabIndex = 4;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBox1.Location = new System.Drawing.Point(228, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(566, 215);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 185);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Edit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number";
            // 
            // textBoxStudentNumber
            // 
            this.textBoxStudentNumber.Location = new System.Drawing.Point(94, 3);
            this.textBoxStudentNumber.Name = "textBoxStudentNumber";
            this.textBoxStudentNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentNumber.TabIndex = 2;
            // 
            // textBoxStudentAddress
            // 
            this.textBoxStudentAddress.Location = new System.Drawing.Point(94, 159);
            this.textBoxStudentAddress.Name = "textBoxStudentAddress";
            this.textBoxStudentAddress.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentAddress.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Address";
            // 
            // textBoxStudentName
            // 
            this.textBoxStudentName.Location = new System.Drawing.Point(94, 29);
            this.textBoxStudentName.Name = "textBoxStudentName";
            this.textBoxStudentName.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentName.TabIndex = 2;
            // 
            // textBoxStudentPhone
            // 
            this.textBoxStudentPhone.Location = new System.Drawing.Point(94, 133);
            this.textBoxStudentPhone.Name = "textBoxStudentPhone";
            this.textBoxStudentPhone.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentPhone.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Birthdate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Phone";
            // 
            // textBoxStudentBirthdate
            // 
            this.textBoxStudentBirthdate.Location = new System.Drawing.Point(94, 81);
            this.textBoxStudentBirthdate.Name = "textBoxStudentBirthdate";
            this.textBoxStudentBirthdate.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentBirthdate.TabIndex = 2;
            // 
            // textBoxStudentGender
            // 
            this.textBoxStudentGender.Location = new System.Drawing.Point(94, 107);
            this.textBoxStudentGender.Name = "textBoxStudentGender";
            this.textBoxStudentGender.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentGender.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Gender";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.dataGridViewModuleCodes);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 237);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(794, 210);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Module Codes";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(85, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(293, 11);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Delete";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(212, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Add";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // dataGridViewModuleCodes
            // 
            this.dataGridViewModuleCodes.AllowDrop = true;
            this.dataGridViewModuleCodes.AllowUserToOrderColumns = true;
            this.dataGridViewModuleCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewModuleCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Module_Code,
            this.Module_Name,
            this.Module_Description});
            this.dataGridViewModuleCodes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewModuleCodes.Location = new System.Drawing.Point(3, 40);
            this.dataGridViewModuleCodes.Name = "dataGridViewModuleCodes";
            this.dataGridViewModuleCodes.ReadOnly = true;
            this.dataGridViewModuleCodes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewModuleCodes.Size = new System.Drawing.Size(788, 167);
            this.dataGridViewModuleCodes.TabIndex = 0;
            this.dataGridViewModuleCodes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewModuleCodes_CellClick);
            // 
            // Module_Code
            // 
            this.Module_Code.HeaderText = "Module_Code";
            this.Module_Code.Name = "Module_Code";
            this.Module_Code.ReadOnly = true;
            // 
            // Module_Name
            // 
            this.Module_Name.HeaderText = "Module_Name";
            this.Module_Name.Name = "Module_Name";
            this.Module_Name.ReadOnly = true;
            // 
            // Module_Description
            // 
            this.Module_Description.HeaderText = "Module_Description";
            this.Module_Description.Name = "Module_Description";
            this.Module_Description.ReadOnly = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Module Code";
            // 
            // textBoxStudentImage
            // 
            this.textBoxStudentImage.Location = new System.Drawing.Point(94, 55);
            this.textBoxStudentImage.Name = "textBoxStudentImage";
            this.textBoxStudentImage.Size = new System.Drawing.Size(100, 20);
            this.textBoxStudentImage.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Image";
            // 
            // FormMutateStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormMutateStudent";
            this.Text = "FormMutateStudent";
            this.Load += new System.EventHandler(this.FormMutateStudent_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModuleCodes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxStudentAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxStudentPhone;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxStudentGender;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxStudentBirthdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxStudentName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStudentNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewModuleCodes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Module_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Module_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Module_Description;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxStudentImage;
    }
}