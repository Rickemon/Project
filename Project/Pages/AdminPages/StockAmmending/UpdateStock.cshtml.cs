using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.AdminPages.StockAmmending
{
    public class UpdateStock : PageModel
    {
        [BindProperty]
        public Stock Items { get; set; }
        public IActionResult OnGet(string ID)
        {
            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            Items = new Stock();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Stock WHERE ProductID = @ID";

                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Items.ProductID = reader.GetInt32(0);
                    Items.ProductName = reader.GetString(1);
                    Items.Price = reader.GetDecimal(2);
                    
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
                command.CommandText = "UPDATE Stock SET ProductName= @ProductName, Price = @Price WHERE ProductID = @ID";

                command.Parameters.AddWithValue("@ID", Items.ProductID);
                command.Parameters.AddWithValue("@ProductName", Items.ProductName);
                command.Parameters.AddWithValue("@Price", Items.Price);

                command.ExecuteNonQuery();
            }

            conn.Close();

            return RedirectToPage("/AdminPages/StockAmmending/ViewStock");
        }

    }
}
