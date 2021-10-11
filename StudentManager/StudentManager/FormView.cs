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
            tabControl1.SelectedTab = tabControl1.TabPages[lastOpentab];
            ProgramInfo.studentAndModuleData = new DatabaseConnection().GetStudentModuleDataView();

            ProgramInfo.selectedStudent = null;
            dataGridViewStudent.DataSource = ProgramInfo.studentAndModuleData.students;
            dataGridViewStudent.Refresh();
            if (ProgramInfo.studentAndModuleData?.students != null)
                foreach (var student in ProgramInfo.studentAndModuleData.students)
                    comboBox1.Items.Add(student.Student_Number);

            ProgramInfo.selectedModule = null;
            dataGridViewModule.DataSource = ProgramInfo.studentAndModuleData.modules;
            dataGridViewModule.Refresh();
            if (ProgramInfo.studentAndModuleData?.modules != null)
                foreach (var module in ProgramInfo.studentAndModuleData.modules)
                    comboBox2.Items.Add(module.Module_Code);

        }

        private class ModuleResource { public string OnlineResource { get; set; } }
        private void DataGridViewModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewModule.SelectedRows.Count > 0)
                try
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
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
        }

        private void DataGridViewStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewStudent.SelectedRows.Count > 0)
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
                catch (Exception exception)
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

        private static int lastOpentab = 0;
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e) { lastOpentab = 0; }

        private void ComboBox2_TextUpdate(object sender, EventArgs e)
        {
            var modules = ProgramInfo.studentAndModuleData?.modules?.FindAll(module => module?.Module_Code == comboBox2.Text);
            dataGridViewModule.DataSource = modules;
            dataGridViewModule.Refresh();
            if (modules.Count > 0)
                DataGridViewModule_CellClick(null, null);
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox2_TextUpdate(null, null);
        }

        private void ComboBox1_TextUpdate(object sender, EventArgs e)
        {
            var students = ProgramInfo.studentAndModuleData?.students?.FindAll(student => student?.Student_Number == comboBox1.Text);
            dataGridViewStudent.DataSource = students;
            dataGridViewStudent.Refresh();
            if (students.Count > 0)
                DataGridViewStudent_CellClick(null, null);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox1_TextUpdate(null, null);
        }
    }
}
