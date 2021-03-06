﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class OrderDetails
    {
        [Key]
        [Display(Name = "OrderID")]
        public int Order_ID { get; set; }
        [Key]
        [Display(Name = "ProductID")]
        public int Product_ID { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }
    }
}
