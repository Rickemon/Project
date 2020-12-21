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

namespace Project.Pages.Customer
{
    public class DeleteCustomer : PageModel
    {
        [BindProperty]
        public Users Customer { get; set; }

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";


        public string SessionName;
        public const string SessionKeyName2 = "SessionName";

        public string SessionID;
        public const string SessionKeyName3 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName4 = "SessionRole";

        public IActionResult OnGet(string ID)
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
