using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StudentManager.Data;

namespace StudentManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (true)
            {
                switch (ProgramInfo.nextForm)
                {
                    case ProgramInfo.Form.Landing: Application.Run(new FormLanding()); break;
                    case ProgramInfo.Form.MutateModule: Application.Run(new FormMutateModule()); break;
                    case ProgramInfo.Form.MutateStudent: Application.Run(new FormMutateStudent()); break;
                    case ProgramInfo.Form.View: Application.Run(new FormView()); break;
                    default: return;
                }
            }
        }
    }
}
