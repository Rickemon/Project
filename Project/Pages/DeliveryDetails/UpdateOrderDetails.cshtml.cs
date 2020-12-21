using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.AdminPages.OrderDetailsAmmending
{
    public class UpdateOrderDetails : PageModel
    {
        [BindProperty]
        public OrderDetails Items { get; set; }
        public IActionResult OnGet(int Order_ID, int Product_ID)
        {
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
                command.Parameters.AddWithValue("@Product_ID", Product_ID);

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
                command.CommandText = "UPDATE OrderDetails SET Quantity = @Quantity WHERE OrderID = @ID AND ProductID = @ProductID  ";

                command.Parameters.AddWithValue("@ID", Items.Order_ID);
                command.Parameters.AddWithValue("@ProductID", Items.Product_ID);
                command.Parameters.AddWithValue("@Quantity", Items.Quantity);

                command.ExecuteNonQuery();
            }

            conn.Close();

            return RedirectToPage("/DeliveryDetails/ViewOrderDetails");
        }

    }
}
