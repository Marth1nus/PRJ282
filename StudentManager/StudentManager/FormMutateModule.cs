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

        private void FormMutateModule_Load(object sender, EventArgs e)
        {

        }
    }
}
