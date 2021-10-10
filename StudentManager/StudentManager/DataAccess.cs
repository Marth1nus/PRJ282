﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime;
using System.IO;
using static StudentManager.Data;

namespace StudentManager
{
    public static class DataAccess
    {
        private static readonly string DatabaseConnectionString = "Data Source=(local); Initial Catalog = BCStudents;";

        public class DatabaseConnection
        {
            private readonly SqlConnection connection = new SqlConnection(DatabaseConnectionString);
            public DatabaseConnection() 
            {
                try { connection.Open(); }
                catch (Exception exception) { Console.WriteLine(exception.Message); }
            }
            ~DatabaseConnection() { connection.Close(); }

            public StudentAndModuleData GetStudentModuleDataView()
            {
                var modules = GetModules();
                var students = GetStudents();
                if (students != null && modules != null)
                {   // scrap copies
                    List<string> Module_Codes = modules.ConvertAll<string>(module => module.Module_Code);
                    foreach (var student in students)
                        student.Modules = modules.FindAll(module => Module_Codes.Contains(module.Module_Code));
                }
                return new StudentAndModuleData() { modules = modules, students = students };
            }

            private string GetModuleSELECT(string tableSource = null)
            {
                var S = tableSource == null ? "" : (tableSource + ".");
                return 
                    $"{S}{nameof(Module.Module_Code)}," +
                    $"{S}{nameof(Module.Module_Name)}," +
                    $"{S}{nameof(Module.Module_Description)}";
            }

            private List<Module> GetModulesFromQuery(string query)
            {
                try
                {
                    List<Module> modules = new List<Module>();
                    SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader();
                    while (reader.Read())
                    {
                        Module newModule = new Module()
                        {
                            Module_Code = $"{reader[nameof(Module.Module_Code)]}",
                            Module_Name = $"{reader[nameof(Module.Module_Name)]}",
                            Module_Description = $"{reader[nameof(Module.Module_Description)]}",
                            Online_resources = new List<string>()
                        };

                        SqlCommand ModuleResourcesCommand = new SqlCommand(
                            $"SELECT ResourceURL FROM ModuleOnlineResource WHERE Module_Code = {newModule.Module_Code}",
                            connection);
                        SqlDataReader ModuleResourcesReader = ModuleResourcesCommand.ExecuteReader();
                        while (ModuleResourcesReader.Read())
                            newModule.Online_resources.Add($"{ModuleResourcesReader["ResourceURL"]}");

                        modules.Add(newModule);
                    }
                    return modules;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }

            public List<Module> GetModules(string whereParam = "TRUE")
            {
                return GetModulesFromQuery($"SELECT {GetModuleSELECT()} FROM Module WHERE {whereParam}");
            }

            private List<Module> GetModulesWithStudents(string whereParam)
            {
                return GetModulesFromQuery(
                    $"SELECT {GetModuleSELECT("M")},SM.{nameof(Student.Student_Number)} " +
                    $"FROM Module M JOIN StudentModule SM " +
                    $"ON M.Moldule_Code = SM.Module_Code " +
                    $"WHERE {whereParam}"
                );
            }

            private List<Module> GetModulesWithStudents(List<string> studentsNumbers)
            {
                string whereParam = "";
                foreach (var studentNumber in studentsNumbers)
                    whereParam += $"SM.{nameof(Student.Student_Number)} = {studentNumber} or ";
                whereParam.Remove(whereParam.Length - " or ".Length, " or ".Length);
                return GetModulesWithStudents(whereParam);
            }

            public List<Module> GetModulesWithStudents(List<Student> students)
            {
                return GetModulesWithStudents(students.ConvertAll<string>(student => student.Student_Number));
            }

            public List<Module> GetModulesWithStudent(Student student)
            {
                return GetModulesWithStudents(new List<Student> { student });
            }



            private string GetStudentSELECT(string tableSource = null)
            {
                var S = tableSource == null ? "" : (tableSource + ".");
                return 
                    $"{S}{nameof(Student.Student_Number          )}," +
                    $"{S}{nameof(Student.Student_Name_and_Surname)}," +
                    $"{S}{nameof(Student.Student_Image           )}," +
                    $"{S}{nameof(Student.DOB                     )}," +
                    $"{S}{nameof(Student.Gender                  )}," +
                    $"{S}{nameof(Student.Phone                   )}," +
                    $"{S}{nameof(Student.Address                 )} ";
            }

            private List<Student> GetStudentsFromQuery(string query)
            {
                try
                {
                    SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader();
                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student newStudent = new Student()
                        {
                            Student_Number = $"{reader[nameof(Student.Student_Number)]}",
                            Student_Name_and_Surname = $"{reader[nameof(Student.Student_Name_and_Surname)]}",
                            Student_Image = $"{reader[nameof(Student.Student_Image)]}",
                            DOB = Convert.ToDateTime(reader[nameof(Student.DOB)]),
                            Gender = $"{reader[nameof(Student.Gender)]}",
                            Phone = $"{reader[nameof(Student.Phone)]}",
                            Address = $"{reader[nameof(Student.Address)]}",
                            Modules = new List<Module>()
                        };
                        newStudent.Modules = GetModulesWithStudent(newStudent);
                        students.Add(newStudent);
                    }
                    return students;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }
            
            public List<Student> GetStudents(string whereParam = "TRUE")
            {
                return GetStudentsFromQuery($"SELECT {GetStudentSELECT()} FROM Student WHERE {whereParam}");
            }

            private List<Student> GetStudentsWithModules(string whereParam)
            {
                return GetStudentsFromQuery(
                        $"SELECT {GetStudentSELECT("S")}, SM.{nameof(Module.Module_Code)} " +
                        "FROM Student S JOIN StudentModule SM " +
                        "ON S.Student_Number = SM.Student_Number " +
                        $"WHERE {whereParam}"
                        );
            }

            public List<Student> GetStudentsWithModules(List<string> moduleCodes)
            {
                string whereParam = "";
                foreach (var moduleCode in moduleCodes)
                    whereParam += $"SM.{nameof(Module.Module_Code)} = {moduleCode} or ";
                whereParam.Remove(whereParam.Length - " or ".Length, " or ".Length);
                return GetStudentsWithModules(whereParam);
            }

            public List<Student> GetStudentsWithModules(List<Module> modules)
            {
                return GetStudentsWithModules(modules.ConvertAll<string>(module => module.Module_Code));
            }

            public List<Student> GetStudentsWithModule(Module module)
            {
                return GetStudentsWithModules(new List<Module> { module });
            }
        }


        /*
        File Handler functions
        */


        const string DataFilePath = "DataFile.TotallyNotAPlainTextFile";

        private static string ScramblePassword(string plainTextPassword)
        {
            Random random = new Random(0xabcdef);

            int lengthMulpipal = 0x20;
            int length = plainTextPassword.Length / lengthMulpipal * lengthMulpipal + lengthMulpipal;
            StringBuilder builder = new StringBuilder(length);

            // copy and scrable plaintextPassword
            for (int i = 0; i < plainTextPassword.Length; ++i) 
                builder.Append(plainTextPassword[i] + (char)random.Next(0xf));

            // padd to constant length to protect against timing attacks and password leaks
            for (int i = plainTextPassword.Length; i < length; ++i) 
                builder.Append((char)random.Next('0', 'z') + (char)random.Next(0xf));

            return builder.ToString();
        }

        public static IEnumerable<(string username, string scrambledPassword)> GetFileData()
        {
            if (!File.Exists(DataFilePath)) yield break;
            using (StreamReader reader = File.OpenText(DataFilePath))
            {
                for (string line = ""; line != null; line = reader.ReadLine())
                {
                    // TO DO: filer whitespace
                    var lineTuple = line.Split(',');
                    if (lineTuple.Length < 2) continue;
                    yield return (lineTuple[0], lineTuple[1]);
                }
                reader.Close();
            }
        }

        public class UserAccessException : Exception
        {
            public UserAccessException(string message) : base(message) { }
        }

        public static LoginToken GetLoginToken(string username, string password)
        {
            string scrambledPassword = ScramblePassword(password);
            foreach (var fileToken in GetFileData())
                if (username == fileToken.username)
                    if (scrambledPassword == fileToken.scrambledPassword)
                        return new LoginToken(username);
                    else return null;
            throw new UserAccessException($"\"{username}\" is not a registered user");
        }

        public static LoginToken AddLoginToken(string username, string password)
        {
            try
            {
                // verify existing token does not exist
                return GetLoginToken(username, password);
            }
            catch (UserAccessException)
            {
                using (StreamWriter writer = File.AppendText(DataFilePath))
                {
                    // TO DO : filer whitespaces from username and password
                    writer.WriteLine($"{username},{ScramblePassword(password)}");
                    writer.Flush();
                    writer.Close();
                }

                bool debugCheck = true; // validate file contents
                if (debugCheck) return GetLoginToken(username, password);

                return new LoginToken(username);
            }
        }
    }
}
