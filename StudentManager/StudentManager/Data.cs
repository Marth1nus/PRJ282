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
            public string Module_Code { get; set; }
            public string Module_Name { get; set; }
            public string Module_Description { get; set; }
            public List<string> Online_resources { get; set; }
        }

        public class Student
        {
            public string Student_Number { get; set; }
            public string Student_Name_and_Surname { get; set; }
            public string Student_Image { get; set; }
            public DateTime DOB { get; set; }
            public string Gender { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public List<Module> Modules { get; set; }
        }

        public class StudentAndModuleData
        {
            public List<Module> modules;
            public List<Student> students;
        }

        public class LoginToken
        {
            readonly public string username;
            public LoginToken(string username) { this.username = username; }
        }

        public static class ProgramInfo
        {
            public enum Form { MutateStudent, MutateModule, View, Landing, Exit }
            public static Form nextForm = Form.Landing;

            public static LoginToken loginToken = null;
            public static StudentAndModuleData studentAndModuleData = null;
            public static Student selectedStudent = null;
            public static Module selectedModule = null;
        }
    }
}
