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
            button2.Text = "Save";

            textBox3.Enabled = ProgramInfo.selectedModule == null;

            textBox1.Enabled =
            richTextBox1.Enabled =

            dataGridViewOnlineResources.Enabled =
            textBox2.Enabled = true;
            button3.Enabled = 
            button4.Enabled = false;

            button1.Enabled = ProgramInfo.selectedModule != null;

            inViewMode = false;

            richTextBox2.Text += 
                $"Please {(ProgramInfo.selectedModule == null ? "enter" : "edit")} then save new module information\n";
        }

        private void ViewMode()
        {
            button2.Text = ProgramInfo.selectedModule != null ? "Edit" : "Create";
            button1.Enabled = ProgramInfo.selectedModule != null;

            textBox3.Enabled =
            textBox1.Enabled =
            richTextBox1.Enabled =

            dataGridViewOnlineResources.Enabled =
            textBox2.Enabled = 
            button3.Enabled = 
            button4.Enabled = false;


            textBox3.Text = ProgramInfo.selectedModule?.Module_Code;
            textBox1.Text = ProgramInfo.selectedModule?.Module_Name;
            richTextBox1.Text = ProgramInfo.selectedModule?.Module_Description;

            dataGridViewOnlineResources.Rows.Clear();
            if (ProgramInfo.selectedModule?.Online_Resources != null)
                foreach (var url in ProgramInfo.selectedModule?.Online_Resources)
                    dataGridViewOnlineResources.Rows.Add(url);

            inViewMode = true;
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
                    Online_Resources = (from DataGridViewRow row in dataGridViewOnlineResources.Rows select row.Cells[0].Value?.ToString()).ToList()
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
            ViewMode();
            if (ProgramInfo.selectedModule == null && false)
                EditMode();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (inViewMode) EditMode();
            else
            {
                ProgramInfo.selectedModule = GetModuleFromFields();
                if (ProgramInfo.selectedModule != null)
                    if (new DataAccess.DatabaseConnection().SetOrAddModule(ProgramInfo.selectedModule))
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

        private void DataGridViewOnlineResources_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var value = dataGridViewOnlineResources.SelectedRows[0].Cells[0].Value;
                button4.Enabled = value != null;
                textBox2.Text = "";
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                button4.Enabled = false;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewOnlineResources.Rows.Remove(dataGridViewOnlineResources.SelectedRows[0]);
                richTextBox2.Text += "Removed URL\n";
                DataGridViewOnlineResources_CellClick(null, null);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                textBox2.Text = "";
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            dataGridViewOnlineResources.Rows.Add(textBox2.Text);
            textBox2.Text = "";
            richTextBox2.Text += "Added URL\n";
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            button3.Enabled = textBox2.Text.Length > 0;
        }
    }
}
