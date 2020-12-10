using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Orders
    {
        [Key]
        [Display(Name = "OrderID")]
        public int OrderID { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}
