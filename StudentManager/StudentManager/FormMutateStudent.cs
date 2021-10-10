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
        }

        private void EditMode()
        {
            button2.Text = "Save";
            button1.Enabled = ProgramInfo.selectedStudent != null;

            textBoxStudentNumber.Enabled = ProgramInfo.selectedStudent == null;
            textBoxStudentName.Enabled =
            textBoxStudentBirthdate.Enabled =
            textBoxStudentGender.Enabled =
            textBoxStudentPhone.Enabled =
            textBoxStudentAddress.Enabled = 
            dataGridViewModuleCodes.Enabled = true;

        }

        private void FormMutateStudent_Load(object sender, EventArgs e)
        {
            ViewMode();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditMode();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProgramInfo.selectedStudent == null) throw new NullReferenceException("Cannot delete null student");
            new DataAccess.DatabaseConnection().setStudent(ProgramInfo.selectedStudent);
        }
    }
}
