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
using static StudentManager.DataAccess;

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
            button2.Text = ProgramInfo.selectedStudent != null ? "Edit" : "Create";
            button1.Enabled = ProgramInfo.selectedStudent != null;

            textBoxStudentNumber.Enabled =
            textBoxStudentName.Enabled =
            textBoxStudentImage.Enabled =
            textBoxStudentBirthdate.Enabled =
            textBoxStudentGender.Enabled =
            textBoxStudentPhone.Enabled =
            textBoxStudentAddress.Enabled =
            dataGridViewModuleCodes.Enabled = 
            comboBox1.Enabled = false;
            button3.Enabled = button4.Enabled = false;


            textBoxStudentNumber.Text = ProgramInfo.selectedStudent?.Student_Number;
            textBoxStudentName.Text = ProgramInfo.selectedStudent?.Student_Name_and_Surname;
            textBoxStudentImage.Text = ProgramInfo.selectedStudent?.Student_Image;
            textBoxStudentBirthdate.Text = ProgramInfo.selectedStudent?.DOB.ToString("yyyy/MM/dd");
            textBoxStudentGender.Text = ProgramInfo.selectedStudent?.Gender;
            textBoxStudentPhone.Text = ProgramInfo.selectedStudent?.Phone;
            textBoxStudentAddress.Text = ProgramInfo.selectedStudent?.Address;

            dataGridViewModuleCodes.Rows.Clear();
            if (ProgramInfo.selectedStudent?.Modules != null)
                foreach (var module in ProgramInfo.selectedStudent.Modules)
                    dataGridViewModuleCodes.Rows.Add(module.Module_Code, module.Module_Name, module.Module_Description);


            comboBox1.Items.AddRange(ProgramInfo.studentAndModuleData.modules.ConvertAll(module => module.Module_Code).ToArray());

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
            textBoxStudentNumber.Enabled =
            textBoxStudentName.Enabled =
            textBoxStudentImage.Enabled =
            textBoxStudentBirthdate.Enabled =
            textBoxStudentGender.Enabled =
            textBoxStudentPhone.Enabled =
            textBoxStudentAddress.Enabled =
            dataGridViewModuleCodes.Enabled =
            comboBox1.Enabled = true;
            button3.Enabled = button4.Enabled = false;


            inViewMode = false;
        }

        private List<Module> GetModulesFromGrid()
        {
            var result =
                (from DataGridViewRow row in dataGridViewModuleCodes.Rows
                select ProgramInfo.studentAndModuleData.modules.Find(
                    module => module.Module_Code == row.Cells[0].Value?.ToString()
                    )
                ).ToList();
            result.RemoveAll(module => module == null);
            return result;
        }

        private Student GetStudentFromFields()
        {
            try
            {
                return new Student()
                {
                    Student_Number = textBoxStudentNumber.Text,
                    Student_Name_and_Surname = textBoxStudentName.Text,
                    Student_Image = textBoxStudentImage.Text,
                    DOB = DateTime.Parse(textBoxStudentBirthdate.Text),
                    Gender = textBoxStudentGender.Text,
                    Phone = textBoxStudentPhone.Text,
                    Address = textBoxStudentAddress.Text,
                    Modules = GetModulesFromGrid()
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
            ViewMode();
            if (ProgramInfo.selectedStudent == null)
                EditMode();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (inViewMode) EditMode();
            else
            {
                ProgramInfo.selectedStudent = GetStudentFromFields();
                if (ProgramInfo.selectedStudent != null)
                    if (new DatabaseConnection().SetOrAddStudent(ProgramInfo.selectedStudent))
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

        private void Button3_Click(object sender, EventArgs e)
        {
            var selectedModule = ProgramInfo.studentAndModuleData.modules.Find(module => module.Module_Code == comboBox1.Text);
            if (selectedModule != null && GetModulesFromGrid()?.Find(module=>module?.Module_Code == selectedModule.Module_Code) == null)
                dataGridViewModuleCodes.Rows.Add(selectedModule.Module_Code, selectedModule.Module_Name, selectedModule.Module_Description);
            button3.Enabled = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewModuleCodes.Rows.Remove(dataGridViewModuleCodes.SelectedRows[0]);
                DataGridViewModuleCodes_CellClick(null, null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void DataGridViewModuleCodes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                button4.Enabled = true;
                comboBox1.Text = dataGridViewModuleCodes.SelectedRows[0].Cells[0].Value?.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                comboBox1.Text = "";
            }
        }

        private void ComboBox1_TextUpdate(object sender, EventArgs e)
        {
            var selectedModule = ProgramInfo.studentAndModuleData.modules.Find(module => module.Module_Code == comboBox1.Text);
            button3.Enabled = (selectedModule != null && GetModulesFromGrid()?.Find(module => module?.Module_Code == selectedModule?.Module_Code) == null);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox1_TextUpdate(null, null);
        }
    }
}
