using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Pages.DatabaseConnections;

namespace Project.Pages.Customer
{
    public class UploadPictureModel : PageModel
    {
        [BindProperty]
        public IFormFile StdFile { get; set; }

        public readonly IWebHostEnvironment _env;

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";

        public string SessionID;
        public const string SessionKeyName2 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName3 = "SessionRole";

        public UploadPictureModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult OnGet()
        {

            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionID) && string.IsNullOrEmpty(SessionRole))
            {
                return RedirectToPage("/Login/Login");
            }
            return Page();




        }

        public IActionResult OnPost()
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName3);

            string FilePath = null;
            var FileToUpload = Path.Combine(_env.WebRootPath, "Files", StdFile.FileName);//this variable consists of file path

            

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

                SqlDataReader reader = command.ExecuteReader(); 
                while (reader.Read())
                {
                    FilePath = reader.GetString(0); 
                }
                var OldFilePath = Path.Join(_env.WebRootPath, FilePath);
                if (FilePath != "\\Files\\default.jpg")
                {
                    
                    System.IO.File.Delete(OldFilePath);
                }

            }


            conn.Close();
            using (var FStream = new FileStream(FileToUpload, FileMode.Create))
            {
                StdFile.CopyTo(FStream);
            }

            conn.Open();
            FilePath = Path.Combine("\\Files", StdFile.FileName);

            using (SqlCommand command = new SqlCommand())
            {

                command.Connection = conn;

                command.CommandText = "UPDATE Users SET ProfilePicture= @ProfilePicture WHERE Username = @SessionUsername";

                command.Parameters.AddWithValue("@SessionUsername", SessionUsername);
                command.Parameters.AddWithValue("@ProfilePicture", FilePath);


                command.ExecuteNonQuery();
                conn.Close();
            }


            conn.Close();

            return RedirectToPage("/index");
        }

    }
}



