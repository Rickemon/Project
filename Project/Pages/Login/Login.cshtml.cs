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

namespace Project.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Users Users { get; set; }
        public string Message { get; set; }

        public string SessionID;


        

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            DatabaseConnection dbstring = new DatabaseConnection(); //creating an object from the class
            string DbConnection = dbstring.DatabaseString(); //calling the method from the class
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Console.WriteLine(Users.Username);
            Console.WriteLine(Users.Password);


            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT Name, Username, Role FROM Users WHERE Username = @UName AND Password = @Pwd";

                command.Parameters.AddWithValue("@UName", Users.Username);
                command.Parameters.AddWithValue("@Pwd", Users.Password);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Users.Name = reader.GetString(0);
                    Users.Username = reader.GetString(1);
                    Users.Role = reader.GetString(2);
                }


            }

            if (!string.IsNullOrEmpty(Users.Name))
            {
                SessionID = HttpContext.Session.Id;
                HttpContext.Session.SetString("sessionID", SessionID);
                HttpContext.Session.SetString("username", Users.Username);
                HttpContext.Session.SetString("name", Users.Name);
               

                if (Users.Role == "User")
                {
                    return RedirectToPage("/UserPages/UserIndex");
                }
                else
                {
                    return RedirectToPage("/AdminPages/AdminIndex");
                }


            }
            else
            {
                Message = "Invalid Username and Password!";
                return Page();
            }



        }
    }
}
