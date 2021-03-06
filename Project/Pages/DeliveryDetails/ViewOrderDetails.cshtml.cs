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

namespace Project.Pages.DeliveryDetails
{
    public class ViewOrderDetails : PageModel
    {
        public List<OrderDetails> Items { get; set; }

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";


        public string SessionName;
        public const string SessionKeyName2 = "SessionName";

        public string SessionID;
        public const string SessionKeyName3 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName4 = "SessionRole";

        public IActionResult OnGet()
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);
            SessionRole = HttpContext.Session.GetString(SessionKeyName4);

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionName) && string.IsNullOrEmpty(SessionID) && string.IsNullOrEmpty(SessionRole))
            {
                return RedirectToPage("/Login/Login");
            }
            if (SessionRole != "Admin")
            {
                return RedirectToPage("/UserPages/UserIndex");
            }



            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT Order_ID, Username, Product_ID, ProductName, Quantity FROM OrderDetails, Orders, Stock WHERE OrderDetails.Order_ID = Orders.OrderID AND OrderDetails.Product_ID= Stock.ProductID";

                SqlDataReader reader = command.ExecuteReader();

                Items = new List<OrderDetails>();

                while (reader.Read())
                {
                    OrderDetails record = new OrderDetails(); 
                    record.Order_ID = reader.GetInt32(0); 
                    record.Username = reader.GetString(1);
                    record.Product_ID = reader.GetInt32(2);
                    record.ProductName = reader.GetString(3);
                    record.Quantity = reader.GetInt32(4); 

                    Items.Add(record); 
                }

                
                reader.Close();
            }
            conn.Close();
            return Page();

        }
    }
}
