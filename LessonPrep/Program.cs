using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonPrep
{
    class Program
    {
        private static int IdColumn = 0;
        private static int NameColumn = 1;

        static void Main(string[] args)
        {
            SqlConnectionStringBuilder bulder = new SqlConnectionStringBuilder();
            
            bulder.UserID = "lecture";
            bulder.Password = "Qwert123";
            bulder.InitialCatalog = "lecture";
            bulder.DataSource = @"192.168.1.145\SQLEXPRESS";
            bulder.IntegratedSecurity = false;
            
            SqlConnection connection = new SqlConnection(bulder.ToString());
            try
            {
                connection.Open();
                Console.WriteLine("Opened!");
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    for (int i = 0; i < 10; i++)
                    {
                        command.CommandText = "INSERT INTO [dbo].[User] ([Name]) VALUES ('Test')";
                        Console.WriteLine("Inser returns: " + command.ExecuteNonQuery());
                    }
                }

                using (SqlCommand select = new SqlCommand())
                {
                    select.Connection = connection;
                    select.CommandText = "Select * from [dbo].[User]";
                    using (SqlDataReader dataReader = select.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            int id = dataReader.GetInt32(IdColumn);
                            string name = dataReader.GetString(NameColumn);
                            Console.WriteLine("Reade UserId={0}, Name={1}", id, name);
                        }
                    }
                }

                using (SqlCommand withParams = new SqlCommand())
                {
                    withParams.Connection = connection;
                    withParams.CommandText = "UPDATE [dbo].[User] SET Name = @Name where Name='Test'";
                    SqlParameter nameParameter = new SqlParameter("@Name", System.Data.SqlDbType.NVarChar);
                    nameParameter.Value = "Test2";
                    withParams.Parameters.Add(nameParameter);
                    Console.WriteLine("Updated {0} values", withParams.ExecuteNonQuery());
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed :" + ex.ToString());
            }
            Console.ReadLine();
        }
    }
}
