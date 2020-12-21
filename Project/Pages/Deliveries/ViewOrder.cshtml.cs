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

namespace Project.Pages.Deliveries
{
    public class ViewOrder : PageModel
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

                SqlDataReader reader = command.ExecuteReader(); 

                Order = new List<Orders>(); 

                while (reader.Read())
                {
                    Orders record = new Orders(); 
                    record.OrderID = reader.GetInt32(0); 
                    record.Username = reader.GetString(1); 
                    record.ScheduledDeliveryDate = reader.GetDateTime(2); 

                    Order.Add(record); 
                }

                reader.Close();
            }


        }

    }
}
