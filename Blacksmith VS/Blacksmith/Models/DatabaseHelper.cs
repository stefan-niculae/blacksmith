using System.Configuration;
using System.Data.SqlClient;

namespace Blacksmith.Models
{
    public class DatabaseHelper
    { 
        public static SqlConnection NewConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            return connection;
        }

        public static void Cleanup(SqlConnection connection, params SqlCommand[] commands)
        {
            foreach (var command in commands)
                command.Dispose();

            connection.Close();
            connection.Dispose();
        }
    }
}