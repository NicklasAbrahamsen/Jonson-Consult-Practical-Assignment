using CConverteren.Models;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CConverteren
{
    public static class DatabaseConnector
    {
        private static readonly string connectionString = "Data Source=localhost;Initial Catalog=Database;Integrated Security=True";

        public static List<Usernames> GetUsernamesStartingWithA()
        {
            List<Usernames> usernamesList = new List<Usernames>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetUsernameStartingWithA", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Usernames user = new Usernames();
                    user.Username = reader["Username"].ToString();
                    usernamesList.Add(user);
                }

                reader.Close();
            }

            return usernamesList;
        }

        public static GetUserDetailsOutput GetUserDetails(Guid userID)
        {
            GetUserDetailsOutput output = new GetUserDetailsOutput();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetUserDetails", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier);
                parameter.Value = userID;
                command.Parameters.Add(parameter);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output.Username = reader["Username"].ToString();
                    output.Email = reader["Email"].ToString();
                    output.Address = reader["Address"].ToString();
                }

                reader.Close();
            }

            return output;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();

                foreach (DataColumn column in table.Columns)
                {
                    PropertyInfo property = obj.GetType().GetProperty(column.ColumnName);
                    if (property != null && row[column] != DBNull.Value)
                    {
                        property.SetValue(obj, row[column], null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}

