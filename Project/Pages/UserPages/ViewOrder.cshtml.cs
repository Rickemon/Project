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
    public class ViewOwnOrder : PageModel
    {
        public List<Orders> Order { get; set; }

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";

        public string SessionID;
        public const string SessionKeyName2 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName3 = "SessionRole";

        public void OnGet()
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName3);


            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Orders WHERE Username = SessionUsername";

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
