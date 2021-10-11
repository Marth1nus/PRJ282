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
        private static string DatabaseConnectionString = "Data Source=(local);Initial Catalog=BCStudents;Integrated Security=true;Pooling=false;";
        public class DatabaseConnectionOperationException : Exception
        {
            public DatabaseConnectionOperationException(string message = "Database Connection Operation Exception") : base(message) { }
        }

        public class DatabaseConnection
        {
            private static string ToSQLString(string cString)
            {
                /* replace: \ => \\, (newline) => \n, " => \" 
                 * invellop in ''
                 */
                return $"\'{cString.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\"", "\\\"")}\'";
            }

            private static string ToSQLString(DateTime cDate)
            {
                return $"\'{cDate:yyyy-MM-dd}\'";
            }

            private readonly SqlConnection connection = new SqlConnection(DatabaseConnectionString);
            public DatabaseConnection() 
            {
                try { connection.Open(); }
                catch (Exception exception) 
                { 
                    Console.WriteLine(exception); 
                    Console.WriteLine("Please Enter a valid connection string : ");
                    DatabaseConnectionString = Console.ReadLine();
                }
            }
            ~DatabaseConnection()
            {
                try { connection.Close(); }
                catch (Exception exception) { Console.WriteLine(exception); }
            }

            /*
             * Database Get Commands
            */

            public StudentAndModuleData GetStudentModuleDataView()
            {
                var modules = GetModules();
                var students = GetStudents();

                modules.RemoveAll(module => module == null);
                if (students != null && modules != null)
                    foreach (var student in students) // merge copies
                    {
                        student.Modules = student.Modules?.Select(sModule => modules.Find(module => module.Module_Code == sModule.Module_Code)).ToList();
                        student.Modules.RemoveAll(module => module == null);
                    }

                return new StudentAndModuleData() { modules = modules, students = students };
            }

            private string GetModuleAttributesString(string tableSource = null)
            {
                var S = tableSource == null ? "" : (tableSource + ".");
                return 
                    $"{S}{nameof(Module.Module_Code)}," +
                    $"{S}{nameof(Module.Module_Name)}," +
                    $"{S}{nameof(Module.Module_Description)}";
            }

            private List<Module> GetModulesFromQuery(string query)
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction("GetModule");
                    List<Module> modules = new List<Module>();
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        modules.Add(new Module()
                        {
                            Module_Code = $"{reader[nameof(Module.Module_Code)]}",
                            Module_Name = $"{reader[nameof(Module.Module_Name)]}",
                            Module_Description = $"{reader[nameof(Module.Module_Description)]}",
                            Online_Resources = new List<string>()
                        });
                    }
                    reader.Close();
                    modules.ForEach(module=>
                    {
                        SqlCommand ModuleResourcesCommand = new SqlCommand(
                            $"SELECT ResourceURL FROM ModuleOnlineResource WHERE {nameof(Module.Module_Code)} = {ToSQLString(module.Module_Code)};",
                            connection, transaction);
                        SqlDataReader ModuleResourcesReader = ModuleResourcesCommand.ExecuteReader();
                        while (ModuleResourcesReader.Read())
                            module.Online_Resources.Add($"{ModuleResourcesReader["ResourceURL"]}");
                        ModuleResourcesReader.Close();
                    });
                    transaction.Commit();
                    modules.RemoveAll(module => module == null);
                    return modules;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    Console.WriteLine($"query that generated exception : {query}");
                    return null;
                }
            }

            public List<Module> GetModules(string whereParam = "TRUE")
            {
                string WHERE = whereParam == "TRUE" ? "" : $"WHERE {whereParam}";
                return GetModulesFromQuery($"SELECT {GetModuleAttributesString()} FROM Module {WHERE};");
            }

            private List<Module> GetModulesWithStudents(string whereParam)
            {
                return GetModulesFromQuery(
                    $"SELECT {GetModuleAttributesString("M")},SM.{nameof(Student.Student_Number)} " +
                    $"FROM Module M JOIN StudentModule SM " +
                    $"ON M.{nameof(Module.Module_Code)} = SM.{nameof(Module.Module_Code)} " +
                    $"WHERE {whereParam};"
                );
            }

            private List<Module> GetModulesWithStudents(List<string> studentsNumbers)
            {
                string whereParam = "";
                foreach (var studentNumber in studentsNumbers)
                    whereParam += $"SM.{nameof(Student.Student_Number)} = {ToSQLString(studentNumber)} or ";
                whereParam = whereParam.Remove(whereParam.Length - " or ".Length, " or ".Length);
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



            private string GetStudentAttributesString(string tableSource = null)
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
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction("GetStudent");
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    SqlDataReader reader = command.ExecuteReader();
                    List<Student> students = new List<Student>();
                    while (reader.Read())
                        students.Add(new Student()
                        {
                            Student_Number = $"{reader[nameof(Student.Student_Number)]}",
                            Student_Name_and_Surname = $"{reader[nameof(Student.Student_Name_and_Surname)]}",
                            Student_Image = $"{reader[nameof(Student.Student_Image)]}",
                            DOB = Convert.ToDateTime(reader[nameof(Student.DOB)]),
                            Gender = $"{reader[nameof(Student.Gender)]}",
                            Phone = $"{reader[nameof(Student.Phone)]}",
                            Address = $"{reader[nameof(Student.Address)]}",
                            Modules = new List<Module>()
                        });
                    reader.Close();
                    transaction.Commit();
                    students.ForEach(student => student.Modules = GetModulesWithStudent(student));
                    return students;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    Console.WriteLine($"query that generated exception : {query}");
                    return null;
                }
            }
            
            public List<Student> GetStudents(string whereParam = null)
            {
                return GetStudentsFromQuery($"SELECT {GetStudentAttributesString()} FROM Student {(whereParam == null ? "" : $"WHERE {whereParam}")};");
            }

            private List<Student> GetStudentsWithModules(string whereParam)
            {
                return GetStudentsFromQuery(
                        $"SELECT {GetStudentAttributesString("S")}, SM.{nameof(Module.Module_Code)} " +
                        $"FROM Student S JOIN StudentModule SM " +
                        $"ON S.{nameof(Student.Student_Number)} = SM.{nameof(Student.Student_Number)} " +
                        $" {(whereParam == null ? "" : $"WHERE {whereParam}")};"
                        );
            }

            private List<Student> GetStudentsWithModules(List<string> moduleCodes)
            {
                string whereParam = "";
                foreach (var moduleCode in moduleCodes)
                    whereParam += $"SM.{nameof(Module.Module_Code)} = {ToSQLString(moduleCode)} or ";
                whereParam = whereParam.Remove(whereParam.Length - " or ".Length, " or ".Length);
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


            /*
             * Database Set/Remove functions
            */


            private bool SetOrAddModuleFromQuery(string query, Module Online_Resources = null)
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction("SetModule");
                    int rowsAffected = new SqlCommand(query, connection, transaction).ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        transaction.Commit();
                        if (!(Online_Resources == null || SetModuleOnlineResource(Online_Resources)))
                            throw new DatabaseConnectionOperationException("Failed to set Online Resources");
                        return true;
                    }
                    else throw new IndexOutOfRangeException($"{rowsAffected} rows affected in SetModule operation");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    Console.WriteLine($"query that generated exception : {query}");
                    return false;
                }
            }

            /*
             *  NOTE : With set or add functions consider using SqlCommand.Parameter.AddWithValue(varName, value)
             *      to reaplace using ToSQLString mothod as the SqlCommand class has that functionality implimented
             *      likely in much more detail and much less lazily. For now as 
             */

            private bool SetModuleOnlineResource(Module module)
            {
                SqlTransaction transaction = null;
                if (module.Online_Resources == null) return false;
                try
                {
                    transaction = connection.BeginTransaction("SetModule");
                    int rowsAffected = new SqlCommand(
                        $"DELETE FROM ModuleOnlineResource WHERE {nameof(Module.Module_Code)} = {ToSQLString(module.Module_Code)}",
                        connection, transaction).ExecuteNonQuery();
                    foreach (string ResourceURL in module.Online_Resources)
                        if (ResourceURL != null && ResourceURL.Length > 0)
                        {
                            rowsAffected = new SqlCommand(
                                $"INSERT INTO ModuleOnlineResource" +
                                $"({nameof(module.Module_Code)}, {nameof(ResourceURL)}) VALUES" +
                                $"({ToSQLString(module.Module_Code)}, {ToSQLString(ResourceURL)})",
                                connection, transaction).ExecuteNonQuery();
                            if (rowsAffected != 1) throw new DatabaseConnectionOperationException($"Failed SetModuleOnlineResource operation");
                        }
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    return false;
                }
            }

            public bool SetOrAddModule(Module module)
            {
                var existingModules = GetModules($"{nameof(Module.Module_Code)} = {ToSQLString(module.Module_Code)}");
                if (existingModules == null || existingModules.Count == 0)
                    return SetOrAddModuleFromQuery(
                        $"INSERT INTO Module ({GetModuleAttributesString()}) VALUES (" +
                        $"{ToSQLString(module.Module_Code)}," +
                        $"{ToSQLString(module.Module_Name)}," +
                        $"{ToSQLString(module.Module_Description)}" +
                        $")",
                        module
                        );
                else if (existingModules.Count == 1)
                    return SetOrAddModuleFromQuery(
                        $"UPDATE Module SET " +
                        $"{nameof(Module.Module_Name)} = {ToSQLString(module.Module_Name)}," +
                        $"{nameof(Module.Module_Description)} = {ToSQLString(module.Module_Description)} " +
                        $"WHERE {nameof(Module.Module_Code)} = {ToSQLString(module.Module_Code)}",
                        module
                        );
                else
                    throw new DatabaseConnectionOperationException("Database had duplicate primary keys in table Module");
            }

            public bool RemoveModule(Module module)
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction("RemoveModule");
                    int rowsAffected = new SqlCommand(
                        $"DELETE FROM ModuleOnlineResource WHERE {nameof(Module.Module_Code)} = {ToSQLString(module.Module_Code)}",
                        connection, transaction).ExecuteNonQuery();
                    rowsAffected = new SqlCommand(
                        $"DELETE FROM Module WHERE {nameof(Module.Module_Code)} = {ToSQLString(module.Module_Code)}",
                        connection, transaction).ExecuteNonQuery();
                    if (rowsAffected != 1) throw new DatabaseConnectionOperationException($"Failed RemoveModule operation");
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    return false;
                }
            } 



            private bool SetOrAddStudentFromQuery(string query, Student setModules = null)
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction("SetStudent");
                    int rowsAffected = new SqlCommand(query, connection, transaction).ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        transaction.Commit();
                        if (setModules == null || SetOrAddStudentModules(setModules))
                            return true;
                        else 
                            throw new DatabaseConnectionOperationException("Failed to set Student's Modules");
                    }
                    else
                        throw new IndexOutOfRangeException($"{rowsAffected} rows affected in SetStudent operation");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    Console.WriteLine($"query that generated exception : {query}");
                    return false;
                }
            }

            private bool SetOrAddStudentModules(Student student)
            {
                SqlTransaction transaction = null;
                string query = "";
                try
                {
                    transaction = connection.BeginTransaction("SetStudentModules");
                    int rowsAffected = new SqlCommand(query =
                        $"DELETE FROM StudentModule WHERE {nameof(Student.Student_Number)} = {ToSQLString(student.Student_Number)}",
                        connection, transaction).ExecuteNonQuery();
                    foreach (var module in student.Modules)
                        if (module != null)
                        {
                            rowsAffected = new SqlCommand(query =
                                $"INSERT INTO StudentModule ({nameof(Student.Student_Number)}, {nameof(Module.Module_Code)}) " +
                                $"VALUES ({ToSQLString(student.Student_Number)}, {ToSQLString(module.Module_Code)})",
                                connection, transaction).ExecuteNonQuery();
                            if (rowsAffected != 1)
                                throw new IndexOutOfRangeException($"failed to add module {module.Module_Code}");
                        }
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    Console.WriteLine($"query that generated exception : {query}");
                    return false;
                }
            }

            public bool SetOrAddStudent(Student student)
            {
                var existingModules = GetStudents($"{nameof(Student.Student_Number)} = {ToSQLString(student.Student_Number)}");
                if (existingModules == null || existingModules.Count == 0)
                    return SetOrAddStudentFromQuery(
                        $"INSERT INTO Student ({GetStudentAttributesString()}) VALUES (" +
                        $"{ToSQLString(student.Student_Number)}," +
                        $"{ToSQLString(student.Student_Name_and_Surname)}," +
                        $"{ToSQLString(student.Student_Image)}," +
                        $"{ToSQLString(student.DOB)}," +
                        $"{ToSQLString(student.Gender)}," +
                        $"{ToSQLString(student.Phone)}," +
                        $"{ToSQLString(student.Address)} " +
                        $")",
                        student
                        );
                else if (existingModules.Count == 1)
                    return SetOrAddStudentFromQuery(
                        $"UPDATE Student SET " +
                        $"{nameof(Student.Student_Name_and_Surname)} = {ToSQLString(student.Student_Name_and_Surname)}," +
                        $"{nameof(Student.Student_Image)} = {ToSQLString(student.Student_Image)}," +
                        $"{nameof(Student.DOB)} = {ToSQLString(student.DOB)}," +
                        $"{nameof(Student.Gender)} = {ToSQLString(student.Gender)}," +
                        $"{nameof(Student.Phone)} = {ToSQLString(student.Phone)}," +
                        $"{nameof(Student.Address)} = {ToSQLString(student.Address)} " +
                        $"WHERE {nameof(Student.Student_Number)} = {ToSQLString(student.Student_Number)}",
                        student
                        );
                else 
                    throw new DatabaseConnectionOperationException("duplicate Student_Numbers returned by database");
            }

            public bool RemoveStudent(Student student)
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction("RemoveStudent");
                    int rowsAffected = new SqlCommand(
                        $"DELETE FROM StudentModule WHERE {nameof(Student.Student_Number)} = {ToSQLString(student.Student_Number)}",
                        connection, transaction).ExecuteNonQuery();
                    rowsAffected = new SqlCommand(
                        $"DELETE FROM Student WHERE {nameof(Student.Student_Number)} = {ToSQLString(student.Student_Number)}",
                        connection, transaction).ExecuteNonQuery();
                    if (rowsAffected != 1) 
                        throw new IndexOutOfRangeException($"Failed RemoveStudent operation");
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    try { transaction?.Rollback(); }
                    catch (Exception rolbackException)
                    {
                        Console.WriteLine(rolbackException.Message);
                    }
                    return false;
                }
            }
        }


        /*
         * File Handler functions
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

        private static bool SafeStringCompare(string left, string right)
        {
            /* Constant time interval, branchless string comparison
             * Safe against most timing attacks for sets of 0x100 characters
            */
            bool result = true; 
            int length = Math.Max(left.Length, right.Length) / 0x100 * 0x100 + 0x100;
            for (int i = 0; i < length; ++i)
            {
                char l = i < left.Length ? left[i] : '\0';
                char r = i < right.Length ? right[i] : '\0';
                result = result && (l == r);
            }
            return result;
        }

        public static LoginToken GetLoginToken(string username, string password)
        {
            string scrambledPassword = ScramblePassword(password);
            foreach (var fileToken in GetFileData())
                if (username == fileToken.username)
                    if (SafeStringCompare(scrambledPassword, fileToken.scrambledPassword))
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
