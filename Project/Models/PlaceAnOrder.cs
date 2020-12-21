using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class PlaceAnOrder
    {
        [Key]
        [Display(Name = "OrderID")]
        public int OrderID { get; set; }
        [Key]
        [Display(Name = "ProductID")]
        public int ProductID { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }


    }
}
