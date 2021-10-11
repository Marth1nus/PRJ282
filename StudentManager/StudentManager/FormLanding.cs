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
    public partial class FormLanding : Form
    {
        public FormLanding()
        {
            ProgramInfo.nextForm = ProgramInfo.Form.Exit;
            InitializeComponent();
        }

        private void HintUsers()
        {
            int itemIndex = 0;
            comboBox1.Items.Clear();
            foreach (var (username, scrambledPassword) in GetFileData())
                comboBox1.Items.Insert(itemIndex++, username);
        }

        private void FormLanding_Load(object sender, EventArgs e)
        {
            HintUsers();
            if (ProgramInfo.loginToken != null)
            {
                labelInfo.Text = "";
                labelLoggedIn.Text = $"Logged in as: {ProgramInfo.loginToken.username}";
                button1.Text = "Log Out";
                button2.Enabled = button3.Enabled = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            HintUsers();
            if (ProgramInfo.loginToken == null)
            {
                try { ProgramInfo.loginToken = GetLoginToken(comboBox1.Text, textBox1.Text); }
                catch (Exception exception)
                {
                    ProgramInfo.loginToken = null;
                    Console.WriteLine(exception);
                }
                if (ProgramInfo.loginToken == null)
                {
                    ProgramInfo.loginToken = null;
                    labelInfo.Text = "Login Failed";
                    button1.Text = "Log In";
                    labelLoggedIn.Text = "";
                    button2.Enabled = button3.Enabled = false;
                }
                else
                {
                    labelInfo.Text = "Login Successful";
                    labelLoggedIn.Text = $"Logged in as: {ProgramInfo.loginToken.username}";
                    button1.Text = "Log Out";
                    button2.Enabled = button3.Enabled = true;
                }
            }
            else
            {
                ProgramInfo.loginToken = null;
                labelInfo.Text = "Logout Successful";
                labelLoggedIn.Text = "";
                button1.Text = "Log In";
                button2.Enabled = button3.Enabled = false;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (AddLoginToken(comboBox1.Text, textBox1.Text) != null)
                    labelInfo.Text = "Registration Successful";
                else
                    labelInfo.Text = "Registration Failed";
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                labelInfo.Text = "Registration Failed";
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (ProgramInfo.loginToken == null)
                labelInfo.Text = "Please LogIn before accessing data";
            else
            {
                ProgramInfo.nextForm = ProgramInfo.Form.View;
                Close();
            }
        }
    }
}
