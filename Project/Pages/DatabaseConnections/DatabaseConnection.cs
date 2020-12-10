using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Pages.DatabaseConnections
{
    public class DatabaseConnection
    {
        public string DatabaseString()
        {
            string DbString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trick\source\repos\Project\Project\Data\Bakery.mdf;Integrated Security=True;Connect Timeout=30";
            return DbString;
        }
    }
}
