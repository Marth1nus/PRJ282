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
            else
            {
#if DEBUG && true
#warning auto login still active
            Console.WriteLine("TEST AUTO LOGIN ENABLED!!!!");
            comboBox1.Text = "TESTUSER";
            textBox1.Text = "TESTUSER";
            Button1_Click(null, null);
            if (button3.Enabled) 
                Button3_Click(null, null);
#endif
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
                    Console.WriteLine(exception.Message);
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
                Console.WriteLine(exception.Message);
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
