using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinderellaCore.Web.Models
{
    public class UpdateAccountModel 
    {
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Enable Import")]
        public bool EnableImport { get; set; }
    }
}