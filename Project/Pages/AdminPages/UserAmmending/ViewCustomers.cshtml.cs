using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.AdminPages.UserAmmending
{ 
     public class ViewCustomers : PageModel
    {
        public List<Users> Customers { get; set; }

        public void OnGet()
        {

            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Users";

                SqlDataReader reader = command.ExecuteReader(); //SqlDataReader is used to read record from a table

                Customers = new List<Users>(); //this object of list is created to populate all records from the table

                while (reader.Read())
                {
                    Users record = new Users(); //a local var to hold a record 
                    record.Username = reader.GetString(0); //getting the first field from the table
                    record.Password = reader.GetString(1); //getting the second field from the table
                    record.Name = reader.GetString(2); //getting the third field from the table
                    record.Address = reader.GetString(3);
                    record.Role = reader.GetString(5);

                    Customers.Add(record); //adding the single record into the list
                }

                // Call Close when done reading.
                reader.Close();
            }


        }
    }
}