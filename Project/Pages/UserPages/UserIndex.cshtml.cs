using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.UserPages
{
    public class UserIndexModel : PageModel
    {
        public string FilePath;

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";

        public string SessionID;
        public const string SessionKeyName2 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName3 = "SessionRole";


        public IActionResult OnGet()
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionRole) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Login/Login");
            }


            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT ProfilePicture FROM Users Where Username = @SessionUsername";

                command.Parameters.AddWithValue("@SessionUsername", SessionUsername);


                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader(); //SqlDataReader is used to read record from a table
                while (reader.Read())
                {
                    FilePath = reader.GetString(0); //getting the first field from the table
                }

                // Call Close when done reading.
                reader.Close();

            }

            if (SessionRole == "Customer")
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/AdminPages/AdminIndex");
            }
        }
    }
}