using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project.Models;
using Project.Pages.DatabaseConnections;


namespace Project.Pages.UserPages
{
    public class PlaceOrderModel : PageModel
    {

        public List<Stock> Items { get; set; }
        [BindProperty]
        public List<PlaceAnOrder> OrderDetails { get; set; }
        [BindProperty]
        public Orders Order { get; set; }


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



            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Stock";

                SqlDataReader reader = command.ExecuteReader();

                Items = new List<Stock>();

                while (reader.Read())
                {
                    Stock record = new Stock();
                    record.ProductID = reader.GetInt32(0);
                    record.ProductName = reader.GetString(1);
                    record.Price = reader.GetDecimal(2);

                    Items.Add(record);
                }


                reader.Close();
            }
            conn.Close();
            if (SessionRole == "Customer")
            {
                return Page();
            }
            else
            {
                return RedirectToPage("/AdminPages/AdminIndex");
            }
        }
        public IActionResult OnPost()
        {
            int OrderID;
            SessionUsername = HttpContext.Session.GetString(SessionKeyName1);
            SessionID = HttpContext.Session.GetString(SessionKeyName2);
            SessionRole = HttpContext.Session.GetString(SessionKeyName3);

            DatabaseConnection dbstring = new DatabaseConnection();
            string DbConnection = dbstring.DatabaseString();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Orders (Username, ScheduledDeliveryDate) VALUES (@Username, @ScheduledDeliveryDate); SELECT CONVERT(int, SCOPE_IDENTITY()) as OrderID; ";

                command.Parameters.AddWithValue("@Username", SessionUsername);
                command.Parameters.AddWithValue("@ScheduledDeliveryDate", Order.ScheduledDeliveryDate);


                OrderID = (int)command.ExecuteScalar();

                Order.OrderID = OrderID;

                conn.Close();
                conn.Open();

                    command.Connection = conn;
                    command.CommandText = @"SELECT * FROM Stock";

                    SqlDataReader reader = command.ExecuteReader();

                    Items = new List<Stock>();

                    while (reader.Read())
                    {
                        Stock record = new Stock();
                        record.ProductID = reader.GetInt32(0);
                        record.ProductName = reader.GetString(1);
                        record.Price = reader.GetDecimal(2);

                        Items.Add(record);
                    }


                    reader.Close();
                
                conn.Close();

                conn.Open();
                command.Connection = conn;
                command.CommandText = @"INSERT INTO OrderDetails (Order_ID, Product_ID, Quantity) VALUES (@Order_ID, @Product_ID, @Quantity)";
                command.Parameters.Add("@Order_ID", SqlDbType.Int);
                command.Parameters.Add("@Product_ID", SqlDbType.Int);
                command.Parameters.Add("@Quantity", SqlDbType.Int);
                

                for (int i = 0; i < Items.Count; i++)
                {
                    command.Parameters["@Order_ID"].Value = Order.OrderID;
                    command.Parameters["@Product_ID"].Value = Items[i].ProductID;
                    command.Parameters["@Quantity"].Value = OrderDetails[i].Quantity;

                    command.ExecuteNonQuery();

                }
                conn.Close();
                return RedirectToPage("/UserPages/UserIndex");
            }
        }
    }
}
