using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinderellaCore.Web.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string PasswordConfirm { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
    }
}