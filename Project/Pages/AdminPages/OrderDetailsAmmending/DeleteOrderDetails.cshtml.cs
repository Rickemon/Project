using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.AdminPages.OrderAmmending
{
    public class DeleteOrder : PageModel
    {
        [BindProperty]
        public Orders Orders { get; set; }
        public IActionResult OnGet(string ID)
        {
            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            Orders = new Orders();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Order WHERE ProductID = @ID";

                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Orders.OrderID = reader.GetInt32(0);
                    Orders.Username = reader.GetString(1);
                    Orders.Date = reader.GetDateTime(2);

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
                command.CommandText = "DELETE Stock WHERE OrderID = @OrderID";
                command.Parameters.AddWithValue("@OrderID", Orders.OrderID);
                command.ExecuteNonQuery();
            }

            conn.Close();
            return RedirectToPage("/Index");
        }


      
    }
}
