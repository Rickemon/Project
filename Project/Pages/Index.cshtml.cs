using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string SessionUsername;
        public const string SessionKeyName1 = "SessionUsername";


        public string SessionName;
        public const string SessionKeyName2 = "SessionName";

        public string SessionID;
        public const string SessionKeyName3 = "SessionID";

        public string SessionRole;
        public const string SessionKeyName4 = "SessionRole";

        public IActionResult OnGet()
        {
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionName = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName4);

            if (string.IsNullOrEmpty(SessionUsername) && string.IsNullOrEmpty(SessionName) && string.IsNullOrEmpty(SessionID) && string.IsNullOrEmpty(SessionRole))
            {

                return Page();
            }
            if (SessionRole == "Customer")
            {
                return RedirectToPage("/UserPages/UserIndex");
            }
            else
            {
                return RedirectToPage("/AdminPages/AdminIndex");
            }


        }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

    }
}
