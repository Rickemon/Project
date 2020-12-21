using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.AdminPages
{
    public class AdminIndexModel : PageModel
    {
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

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionID) && string.IsNullOrEmpty(SessionRole))
            {

                return RedirectToPage("/Index");
            }
            if (SessionRole == "Customer")
            {
                return RedirectToPage("/UserPages/UserIndex");
            }
            else
            {
                return Page(); 
            }

        }

    }
}
