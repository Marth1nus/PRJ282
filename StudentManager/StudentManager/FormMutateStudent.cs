using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StudentManager.Data;

namespace StudentManager
{
    public partial class FormMutateStudent : Form
    {
        public FormMutateStudent()
        {
            ProgramInfo.nextForm = ProgramInfo.Form.View;
            if (ProgramInfo.loginToken == null) throw new NullReferenceException();
            InitializeComponent();
        }

        ~FormMutateStudent()
        {
            ProgramInfo.selectedStudent = null;
        }

        private bool inViewMode = false;
        private void ViewMode()
        {
            button2.Text = "Edit";
            button1.Enabled = ProgramInfo.selectedStudent != null;

            textBoxStudentNumber.Enabled =
            textBoxStudentName.Enabled =
            textBoxStudentBirthdate.Enabled =
            textBoxStudentGender.Enabled =
            textBoxStudentPhone.Enabled =
            textBoxStudentAddress.Enabled =
            dataGridViewModuleCodes.Enabled = false;


            textBoxStudentNumber.Text = ProgramInfo.selectedStudent?.Student_Number;
            textBoxStudentName.Text = ProgramInfo.selectedStudent?.Student_Name_and_Surname;
            textBoxStudentBirthdate.Text = ProgramInfo.selectedStudent?.DOB.ToString("yyyy/MM/dd");
            textBoxStudentGender.Text = ProgramInfo.selectedStudent?.Gender;
            textBoxStudentPhone.Text = ProgramInfo.selectedStudent?.Phone;
            textBoxStudentAddress.Text = ProgramInfo.selectedStudent?.Address;
            dataGridViewModuleCodes.DataSource = ProgramInfo.selectedStudent?.Modules;

            inViewMode = true;
        }

        private void EditMode()
        {
            button2.Text = "Save";
            button1.Enabled = ProgramInfo.selectedStudent != null;

            richTextBox1.Text += ProgramInfo.selectedStudent == null ?
                "Please enter new student's information\n" :
                "Modify This student's information\n";

            textBoxStudentNumber.Enabled = ProgramInfo.selectedStudent == null;
            textBoxStudentName.Enabled =
            textBoxStudentBirthdate.Enabled =
            textBoxStudentGender.Enabled =
            textBoxStudentPhone.Enabled =
            textBoxStudentAddress.Enabled = 
            dataGridViewModuleCodes.Enabled = true;

            inViewMode = false;
        }

        private Student GetStudentFromFields()
        {
            try
            {
                return new Student()
                {
                    Student_Number = textBoxStudentNumber.Text,
                    Student_Name_and_Surname = textBoxStudentName.Text,
                    DOB = DateTime.Parse(textBoxStudentBirthdate.Text),
                    Gender = textBoxStudentGender.Text,
                    Phone = textBoxStudentPhone.Text,
                    Address = textBoxStudentAddress.Text,
                    Modules = (from DataGridViewRow row in dataGridViewModuleCodes.Rows select row.DataBoundItem)
                        .Select(item => item as Module).ToList()
                };
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        }

        private void FormMutateStudent_Load(object sender, EventArgs e)
        {
            if (ProgramInfo.selectedStudent != null) 
                ViewMode();
            EditMode();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (inViewMode) EditMode();
            else
            {
                ProgramInfo.selectedStudent = GetStudentFromFields();
                if (ProgramInfo.selectedStudent != null)
                    if (new DataAccess.DatabaseConnection().SetStudent(ProgramInfo.selectedStudent))
                    {
                        richTextBox1.Text += "Student update success\n";
                        ViewMode();
                    }
                    else
                        richTextBox1.Text += "Student update Failed\n";
                else
                    richTextBox1.Text += "Some Fields do not contain valid data\n";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ProgramInfo.selectedStudent = GetStudentFromFields();
            if (ProgramInfo.selectedStudent != null)
                if (new DataAccess.DatabaseConnection().RemoveStudent(ProgramInfo.selectedStudent))
                {
                    richTextBox1.Text += "Deletion Successful\n";
                    ProgramInfo.selectedStudent = null;
                    ViewMode();
                }
                else
                    richTextBox1.Text += "Deletion Failed\n";
        }
    }
}
