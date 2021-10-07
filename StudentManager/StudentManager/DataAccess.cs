using System;
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
        private static string DatabaseConnectionString = "Data Source=(local); Initial Catalog = BCStudents;";

        public class DatabaseConnection
        {
            private SqlConnection connection = new SqlConnection(DatabaseConnectionString);
            public DatabaseConnection() 
            {
                try { connection.Open(); }
                catch (Exception exception) { Console.WriteLine(exception.Message); }
            }
            ~DatabaseConnection() { connection.Close(); }

            public StudentModuleDataView GetStudentModuleDataView()
            {
                var modules = GetModules();
                var students = GetStudents();
                return new StudentModuleDataView() { modules = modules, students = students };
            }

            private List<Module> GetModulesFromReader(SqlDataReader reader)
            {
                List<Module> modules = new List<Module>();
                while (reader.Read())
                {
                    Module newModule = new Module()
                    {
                        Module_Code = $"{reader["Module_Code"]}",
                        Module_Name = $"{reader["Module_Name"]}",
                        Module_Description = $"{reader["Module_Description"]}",
                        Online_resources = new List<string>()
                    };

                    SqlCommand ModuleResourcesCommand = new SqlCommand(
                        $"SELECT ResourceURL FROM Module_Online_resource WHERE Module_Code = {newModule.Module_Code}",
                        connection);
                    SqlDataReader ModuleResourcesReader = ModuleResourcesCommand.ExecuteReader();
                    while (ModuleResourcesReader.Read())
                        newModule.Online_resources.Add($"{ModuleResourcesReader["ResourceURL"]}");

                    modules.Add(newModule);
                }
                return modules;
            }

            public List<Module> GetModules(string whereParam = "TRUE")
            {
                try
                {
                    SqlCommand command = new SqlCommand(
                        "SELECT " +
                        "Module_Code," +
                        "Module_Name," +
                        "Module_Description " +
                        $"FROM Module WHERE {whereParam}",
                        connection);
                    return GetModulesFromReader(command.ExecuteReader());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }

            private List<Module> GetModulesWithStudents(string whereParam)
            {
                try
                {
                    SqlCommand command = new SqlCommand(
                            "SELECT " +
                            "M.Module_Code," +
                            "M.Module_Name," +
                            "M.Module_Description " +
                            "FROM Module M JOIN StudentModule SM " +
                            "ON M.Moldule_Code = SM.Module_Code " +
                            $"WHERE {whereParam}",
                        connection);
                    return GetModulesFromReader(command.ExecuteReader());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }

            public List<Module> GetModulesWithStudents(List<string> studentsNumbers)
            {
                string whereParam = "";
                foreach (var studentNumber in studentsNumbers)
                    whereParam += $"Student_Number = {studentNumber} or ";
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



            private List<Student> GetStudentsFromReader(SqlDataReader reader)
            {
                List<Student> students = new List<Student>();
                while (reader.Read())
                {
                    Student newStudent = new Student()
                    {
                        Student_Number           = $"{reader["Student_Number"]}",
                        Student_Name_and_Surname = $"{reader["Student_Name_and_Surname"]}",
                        Student_Image            = $"{reader["Student_Image"]}",
                        DOB                      = Convert.ToDateTime(reader["DOB"]),
                        Gender                   = $"{reader["Gender"]}",
                        Phone                    = $"{reader["Phone"]}",
                        Address                  = $"{reader["Address"]}",
                        Module_Codes             = new List<string>()
                    };

                    SqlCommand getModuleCodesCommand = new SqlCommand(
                        $"SELECT Module_Code FROM StudentModule WHERE Student_Number = {newStudent.Student_Number}",
                        connection);
                    SqlDataReader moduleCodesReader = getModuleCodesCommand.ExecuteReader();
                    while (moduleCodesReader.Read())
                        newStudent.Module_Codes.Add($"{moduleCodesReader["Module_Code"]}");

                    students.Add(newStudent);
                }
                return students;
            }
            
            public List<Student> GetStudents(string whereParam = "TRUE")
            {
                try
                {
                    SqlCommand command = new SqlCommand(
                        "SELECT " +
                        "Student_Number," +
                        "Student_Name_and_Surname," +
                        "Student_Image," +
                        "DOB," +
                        "Gender," +
                        "Phone," +
                        "Address " +
                        $"FROM Student WHERE {whereParam}",
                        connection);
                    return GetStudentsFromReader(command.ExecuteReader());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }

            private List<Student> GetStudentsWithModules(string whereParam)
            {
                try
                {
                    SqlCommand command = new SqlCommand(
                            "SELECT " +
                            "S.Student_Number," +
                            "S.Student_Name_and_Surname," +
                            "S.Student_Image," +
                            "S.DOB," +
                            "S.Gender," +
                            "S.Phone," +
                            "S.Address," +
                            "SM.Module_Code " +
                            "FROM Student S JOIN StudentModule SM " +
                            "ON S.Student_Number = SM.Student_Number " +
                            $"WHERE {whereParam}",
                        connection);
                    return GetStudentsFromReader(command.ExecuteReader());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
            }

            public List<Student> GetStudentsWithModules(List<string> moduleCodes)
            {
                string whereParam = "";
                foreach (var moduleCode in moduleCodes)
                    whereParam += $"Student_Number = {moduleCode} or ";
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



        const string DataFilePath = "DataFile.TotallyNotAPlainTextFile";

        private static string ScramblePassword(string plainTextPassword)
        {
            /*  Here is where something like salt and pepper 
             *  scrambles can be implimented to protect user data.
             *  As I am one person I will not be implimenting anything
             *  like that without libraries for this project
             */
            Random random = new Random(0xabcdef);
            char[] charArray = plainTextPassword.ToCharArray();
            for (int i = 0; i < charArray.Length; ++i)
                charArray[i] += (char)random.Next(16);
            return $"{charArray}";
        }

        private static IEnumerable<(string username, string scrambledPassword)> GetFileData()
        {
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

        public static LoginToken GetLoginToken(string username, string password)
        {
            string scrambledPassword = ScramblePassword(password);
            foreach (var fileToken in GetFileData())
                if (username == fileToken.username)
                    if (scrambledPassword == fileToken.scrambledPassword)
                        return new LoginToken(username);
                    else break;
            return null;
        }

        public static LoginToken AddLoginToken(string username, string password)
        {
            LoginToken existingToken = GetLoginToken(username, password);
            if (existingToken != null) return existingToken;

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
