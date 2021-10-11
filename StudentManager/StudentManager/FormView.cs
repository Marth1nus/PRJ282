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
    public partial class FormView : Form
    {
        public FormView()
        {
            ProgramInfo.nextForm = ProgramInfo.Form.Landing;
            if (ProgramInfo.loginToken == null) throw new NullReferenceException();
            InitializeComponent();
        }

        private void FormView_Load(object sender, EventArgs e)
        {
            ProgramInfo.studentAndModuleData = new DatabaseConnection().GetStudentModuleDataView();
            ProgramInfo.studentAndModuleData = new StudentAndModuleData()
            {
                students = new List<Student>
                {
                    new Student(){ Student_Number = "123", Student_Name_and_Surname = "number0" },
                    new Student(){ Student_Number = "345", Student_Name_and_Surname = "number1" },
                    new Student(){ Student_Number = "456", Student_Name_and_Surname = "number2", DOB = DateTime.Now },
                },
                modules = new List<Module>
                {
                    new Module(){ Module_Code = "prgasd", Module_Name = "as534f", Module_Description = "hefd2345h" },
                    new Module(){ Module_Code = "pr123gasd", Module_Name = "123asf", Module_Description = "hefd2345h" },
                    new Module(){ Module_Code = "prga123213sd", Module_Name = "as456f", Module_Description = "hef2345dh" },
                    new Module(){ Module_Code = "prg321asd", Module_Name = "asf6345", Module_Description = "hefd2345h", 
                        Online_Resources = new List<string>
                        {
                            "https://Place1.com",
                            "https://Place2.com",
                            "https://Place3.com",
                            "https://Place4.com",
                        }
                    }
                }
            };
            ProgramInfo.studentAndModuleData.students[0].Modules = ProgramInfo.studentAndModuleData.modules;

            ProgramInfo.selectedStudent = null;
            dataGridViewStudent.DataSource = ProgramInfo.studentAndModuleData.students;
            dataGridViewStudent.Refresh();

            ProgramInfo.selectedModule = null;
            dataGridViewModule.DataSource = ProgramInfo.studentAndModuleData.modules;
            dataGridViewModule.Refresh(); 
        }

        private class ModuleResource { public string OnlineResource { get; set; } }
        private void DataGridViewModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Module module = ProgramInfo.selectedModule = dataGridViewModule.SelectedRows[0].DataBoundItem as Module;
            textBox3.Text = module.Module_Code;
            textBox1.Text = module.Module_Name;
            richTextBox1.Text = module.Module_Description;
            dataGridViewOnlineResources.DataSource = 
                module.Online_Resources?.ConvertAll<ModuleResource>(resource => new ModuleResource() { OnlineResource = resource });
            dataGridViewOnlineResources.Refresh();
            button2.Enabled = module != null;
        }

        private void DataGridViewStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Student student = ProgramInfo.selectedStudent = dataGridViewStudent.SelectedRows[0].DataBoundItem as Student;
                textBoxStudentNumber.Text = student.Student_Number;
                textBoxStudentName.Text = student.Student_Name_and_Surname;
                textBoxStudentBirthdate.Text = student.DOB.ToString("yyyy/MM/dd");
                textBoxStudentGender.Text = student.Gender;
                textBoxStudentPhone.Text = student.Phone;
                textBoxStudentAddress.Text = student.Address;
                dataGridViewModuleCodes.DataSource = student.Modules;
                dataGridViewModuleCodes.Refresh();
                button4.Enabled = student != null;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ProgramInfo.selectedStudent = null;
            ProgramInfo.nextForm = ProgramInfo.Form.MutateStudent;
            Close();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            ProgramInfo.nextForm = ProgramInfo.Form.MutateStudent;
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ProgramInfo.selectedModule = null;
            ProgramInfo.nextForm = ProgramInfo.Form.MutateModule;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ProgramInfo.nextForm = ProgramInfo.Form.MutateModule;
            Close();
        }
    }
}
