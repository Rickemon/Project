using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.AdminPages.OrderAmmending
{
    public class ViewOrderModel : PageModel
    {
        public List<Orders> Order { get; set; }
        

        public void OnGet()
        {
            

            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Orders";

                SqlDataReader reader = command.ExecuteReader(); //SqlDataReader is used to read record from a table

                Order = new List<Orders>(); //this object of list is created to populate all records from the table

                while (reader.Read())
                {
                    Orders record = new Orders(); //a local var to hold a record 
                    record.OrderID = reader.GetInt32(0); //getting the first field from the table
                    record.Username = reader.GetString(1); //getting the second field from the table
                    record.Date = reader.GetDateTime(2); //getting the third field from the table

                    Order.Add(record); //adding the single record into the list
                }

                // Call Close when done reading.
                reader.Close();
            }


        }

    }
}
