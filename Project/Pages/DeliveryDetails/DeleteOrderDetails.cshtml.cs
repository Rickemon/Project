using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.OrderAttributes
{
    public class DeleteOrderDetails : PageModel
    {
        [BindProperty]
        public OrderDetails Items { get; set; }

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";


        public string SessionName;
        public const string SessionKeyName2 = "SessionName";

        public string SessionID;
        public const string SessionKeyName3 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName4 = "SessionRole";

        public IActionResult OnGet(int Order_ID, int Product_ID)
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);
            SessionRole = HttpContext.Session.GetString(SessionKeyName4);

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionName) && string.IsNullOrEmpty(SessionID) && string.IsNullOrEmpty(SessionRole))
            {
                return RedirectToPage("/Login/Login");
            }

            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            Items = new OrderDetails();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT Order_ID, Username, Product_ID, ProductName, Quantity FROM OrderDetails, Orders, Stock   
                                        WHERE OrderDetails.Order_ID = Orders.OrderID AND OrderDetails.Product_ID= Stock.ProductID 
                                        AND Order_ID = @Order_ID AND Product_ID = @Product_ID";

                command.Parameters.AddWithValue("@Order_ID", Order_ID);
                command.Parameters.AddWithValue("@ProductID", Product_ID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Items.Order_ID = reader.GetInt32(0);
                    Items.Username = reader.GetString(1);
                    Items.Product_ID = reader.GetInt32(2);
                    Items.ProductName = reader.GetString(3);
                    Items.Quantity = reader.GetInt32(4);

                }


            }

            conn.Close();

            return Page();
        }
        public IActionResult OnPost()
        {
            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "DELETE OrderDetails WHERE Order_ID = @Order_ID AND Product_ID = @Product_ID";
                command.Parameters.AddWithValue("@Order_ID", Items.Order_ID);
                command.Parameters.AddWithValue("@ProductID", Items.Product_ID);
                command.ExecuteNonQuery();
            }

            conn.Close();
            return RedirectToPage("/DeliveryDetails/ViewOrderDetails");
        }


      
    }
}
