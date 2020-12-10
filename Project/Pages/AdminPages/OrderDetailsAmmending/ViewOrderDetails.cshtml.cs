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

namespace Project.Pages.AdminPages.OrderDetailsAmmending
{
    public class ViewOrderDetailsModel : PageModel
    {
        public List<OrderDetails> Orders { get; set; }
        

        public void OnGet()
        {
            

            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM OrderDetails";

                SqlDataReader reader = command.ExecuteReader(); //SqlDataReader is used to read record from a table

                Orders = new List<OrderDetails>(); //this object of list is created to populate all records from the table

                while (reader.Read())
                {
                    OrderDetails record = new OrderDetails(); //a local var to hold a record 
                    record.OrderID = reader.GetInt32(0); //getting the first field from the table
                    record.ProductID = reader.GetInt32(1); //getting the second field from the table
                    record.Quantity = reader.GetInt32(2); //getting the third field from the table

                    Orders.Add(record); //adding the single record into the list
                }

                // Call Close when done reading.
                reader.Close();
            }


        }
    }
}
