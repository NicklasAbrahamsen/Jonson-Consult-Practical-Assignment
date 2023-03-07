/*/
using CConverteren.Models;
using Database.Models;
using System;
using System.Data.SqlClient;

namespace CConverteren
{
    class Program
    {
        static void Main(string[] args)
        {
            // Opret en ny bruger
            Usernames username = new Usernames();
            username.Username = "Mads sohn";
            Emails email = new Emails();
            email.Email = "mads.com";
            Adresses adress = new Adresses();
            adress.Adress = "annemonevej 25";

            // Kalder stored proceduren til at indsætte brugeren
            InsertUser(username, email, adress);
        }

        public static void InsertUser(Usernames username, Emails email, Adresses adress)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Database;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand("InsertUser", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter usernameParam = new SqlParameter("@Username", System.Data.SqlDbType.NVarChar, 50);
                usernameParam.Value = username.Username;
                command.Parameters.Add(usernameParam);

                SqlParameter emailParam = new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 50);
                emailParam.Value = email.Email;
                command.Parameters.Add(emailParam);

                SqlParameter adressParam = new SqlParameter("@Adress", System.Data.SqlDbType.NVarChar, 100);
                adressParam.Value = adress.Adress;
                command.Parameters.Add(adressParam);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}

/*/