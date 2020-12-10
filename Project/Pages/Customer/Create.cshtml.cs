using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;

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
            string DbConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trick\source\repos\Project\Project\Data\Bakery.mdf;Integrated Security=True";
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Users (Username, Password, Name, Address) VALUES (@Username, @Password, @Name, @Address)";

                command.Parameters.AddWithValue("@Username", Users.Username);
                command.Parameters.AddWithValue("@Password", Users.Password);
                command.Parameters.AddWithValue("@Name", Users.Name);
                command.Parameters.AddWithValue("@Address", Users.Address);


                Console.WriteLine(Users.Username);
                Console.WriteLine(Users.Password);
                Console.WriteLine(Users.Name);
                Console.WriteLine(Users.Address);



                command.ExecuteNonQuery();
            }


            return RedirectToPage("/Index");
        }

    }
}
