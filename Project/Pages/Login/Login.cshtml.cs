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
        public UserLogin Login { get; set; }
        public string Message { get; set; }

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";

        public string SessionID;
        public const string SessionKeyName2 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName3 = "SessionRole";




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

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT Role FROM Users WHERE Username = @UName AND Password = @Pwd";

                command.Parameters.AddWithValue("@UName", Login.Username);
                command.Parameters.AddWithValue("@Pwd", Login.Password);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Login.Role = reader.GetString(0);
                }


            }

            if (!string.IsNullOrEmpty(Login.Role))
            {
                SessionID = HttpContext.Session.Id;
                HttpContext.Session.SetString("SessionUsername", Login.Username);
                HttpContext.Session.SetString("SessionID", SessionID);
                HttpContext.Session.SetString("SessionRole", Login.Role);

                if (Login.Role == "Customer")
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
                Message = "Invalid Username or Password!";
                return Page();
            }



        }
        public IActionResult OnGet()
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionID) && string.IsNullOrEmpty(SessionRole))
            {
                
                return Page();
            }
            if (SessionRole == "Customer")
            {
                return RedirectToPage("/UserPages/UserIndex");
            }
            else {
                return RedirectToPage("/AdminPages/AdminIndex");
            }
            

        }
    }
}
