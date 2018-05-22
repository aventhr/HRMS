using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AventWithMVC.Models
{
    public class Login
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string Confirmpassword { get; set; }

        public int userroleid { get; set; }

       
    }
}