using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.Customer
{
    public class Create : PageModel
    {
        [BindProperty]
        public Users Users { get; set; }
        public void OnGet()
        {
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
                command.CommandText = @"INSERT INTO Users (Username, Password, Name, Address, Role) VALUES (@Username, @Password, @Name, @Address, @Role)";

                command.Parameters.AddWithValue("@Username", Users.Username);
                command.Parameters.AddWithValue("@Password", Users.Password);
                command.Parameters.AddWithValue("@Name", Users.Name);
                command.Parameters.AddWithValue("@Address", Users.Address);
                command.Parameters.AddWithValue("@Role", Users.Role);

                command.ExecuteNonQuery();
            }


            return RedirectToPage("/Login/Login");
        }

    }
}
