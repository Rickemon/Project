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
    public class DeleteCustomerModel : PageModel
    {
        [BindProperty]
        public Users Customer { get; set; }
        public IActionResult OnGet(string ID)
        {
            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            Customer = new Users();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Users WHERE Username = @ID";

                command.Parameters.AddWithValue("@ID", ID);
                Console.WriteLine("The Username : " + ID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Customer.Username = reader.GetString(0);
                    Customer.Password = reader.GetString(1);
                    Customer.Name = reader.GetString(2);
                    Customer.Address = reader.GetString(3);
                    Customer.Role = reader.GetString(5);
                    Customer.OriginalUserName = Customer.Username;
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
                command.CommandText = "DELETE Users WHERE Username = @Username";
                command.Parameters.AddWithValue("@Username", Customer.Username);
                command.ExecuteNonQuery();
            }

            conn.Close();
            return RedirectToPage("/Index");
        }


      
    }
}
