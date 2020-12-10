using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Stock
    {
        [Key]
        [Display(Name = "ProductID")]
        public int ProductID { get; set; }
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }


    }

}

