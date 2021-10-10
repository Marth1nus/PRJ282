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
    public partial class FormMutateModule : Form
    {
        public FormMutateModule()
        {
            ProgramInfo.nextForm = ProgramInfo.Form.View;
            if (ProgramInfo.loginToken == null) throw new NullReferenceException();
            InitializeComponent();
        }

        ~FormMutateModule()
        {
            ProgramInfo.selectedModule = null;
        }

        private class ModuleResource { public string OnlineResource { get; set; } }
        private bool inViewMode = false;
        private void EditMode()
        {
            inViewMode = false;
            textBox3.Enabled =
            textBox1.Enabled =
            richTextBox1.Enabled =

            dataGridViewOnlineResources.Enabled =
            textBox2.Enabled =
            button3.Enabled =
            button4.Enabled = true;

            button1.Enabled = ProgramInfo.selectedModule != null;
            button2.Text = "Save";
        }

        private void ViewMode()
        {
            inViewMode = true;
            textBox3.Enabled =
            textBox1.Enabled =
            richTextBox1.Enabled =

            dataGridViewOnlineResources.Enabled =
            textBox2.Enabled =
            button3.Enabled = 
            button4.Enabled = false;

            button1.Enabled = ProgramInfo.selectedModule != null;
            button2.Text = "Edit";

            textBox3.Text = ProgramInfo.selectedModule?.Module_Code;
            textBox1.Text = ProgramInfo.selectedModule?.Module_Name;
            richTextBox1.Text = ProgramInfo.selectedModule?.Module_Description;
            dataGridViewOnlineResources.Rows.Clear();
            foreach (var url in ProgramInfo.selectedModule?.Online_Resources)
                dataGridViewOnlineResources.Rows.Add(url);
        }

        private Module GetModuleFromFields()
        {
            try
            {
                return new Module()
                {
                    Module_Code = textBox3.Text,
                    Module_Name = textBox1.Text,
                    Module_Description = richTextBox1.Text,
                    Online_Resources = (from DataGridViewRow row in dataGridViewOnlineResources.Rows select row.Cells[0].Value as string).ToList()
                };
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        }

        private void FormMutateModule_Load(object sender, EventArgs e)
        {
            if (ProgramInfo.selectedModule != null)
                ViewMode();
            EditMode();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (inViewMode) EditMode();
            else
            {
                ProgramInfo.selectedModule = GetModuleFromFields();
                if (ProgramInfo.selectedModule != null)
                    if (new DataAccess.DatabaseConnection().SetModule(ProgramInfo.selectedModule))
                    {
                        richTextBox2.Text += "Module update success\n";
                        ViewMode();
                    }
                    else
                        richTextBox2.Text += "Module update Failed\n";
                else
                    richTextBox2.Text += "Some Fields do not contain valid data\n";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ProgramInfo.selectedModule = GetModuleFromFields();
            if (ProgramInfo.selectedModule != null)
                if (new DataAccess.DatabaseConnection().RemoveModule(ProgramInfo.selectedModule))
                {
                    richTextBox2.Text += "Deletion Successful\n";
                    ProgramInfo.selectedModule = null;
                    ViewMode();
                }
                else
                    richTextBox2.Text += "Deletion Failed\n";
        }

        private DataGridViewRow selectedModuleResourceRow = null;
        private void DataGridViewOnlineResources_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedModuleResourceRow = dataGridViewOnlineResources.SelectedRows[0];
                textBox2.Text = selectedModuleResourceRow.Cells[0].Value as string;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                selectedModuleResourceRow = null;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedModuleResourceRow != null)
                    dataGridViewOnlineResources.Rows.Remove(selectedModuleResourceRow);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                selectedModuleResourceRow = null;
                textBox2.Text = "";
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (selectedModuleResourceRow == null)
                dataGridViewOnlineResources.Rows.Add(textBox2.Text);
            else
                selectedModuleResourceRow.SetValues(textBox2.Text);
            selectedModuleResourceRow = null;
            textBox2.Text = "";
        }
    }
}
