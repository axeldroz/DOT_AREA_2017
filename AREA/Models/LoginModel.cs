using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AREA.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace AREA.Models
{
    public  class LoginModel
    {
        public void AddUser(user ToTest)
        {
            using (AreaEntities db = new AreaEntities())
            {
                user tmp = new user { Id = 1 };
                db.users.Add(tmp);
                db.SaveChanges();
            }
        }
    }

    public class VerifyRegister
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password combination is wrong")]
        public string ConfirmPassword { get; set; }
    }

    public class VerifyLogin
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}