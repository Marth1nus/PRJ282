using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    public static class Data
    {
        public class Module
        {
            public string Module_Code;
            public string Module_Name;
            public string Module_Description;
            public List<string> Online_resources;
        }

        public class Student
        {
            public string Student_Number;
            public string Student_Name_and_Surname;
            public string Student_Image;
            public DateTime DOB;
            public string Gender;
            public string Phone;
            public string Address;
            public List<string> Module_Codes;
        }

        public class StudentModuleDataView
        {
            public List<Module> modules;
            public List<Student> students;

            public List<Module> GetStudentModules(Student student)
            {
                return modules.FindAll(module => student.Module_Codes.Contains(module.Module_Code));
            }
        }

        public class LoginToken
        {
            readonly public string username;
            public LoginToken(string username) { this.username = username; }
        }
    }
}
