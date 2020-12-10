using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Users
    {
        [Key]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "ProfilePicture")]
        public string ProfilePicture { get; set; }
        [Display(Name = "OriginalUserName")]
        public string OriginalUserName { get; set; }
        [Display(Name = "Role")]
        public string Role { get; set; }


    }



}
