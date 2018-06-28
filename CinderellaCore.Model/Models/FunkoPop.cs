using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CinderellaCore.Model.Models
{
    public class FunkoPop : BaseItem
    {
        [Required]
        public string Series { get; set; }

        [Required]
        [DisplayName("Pop Line")]
        public string PopLine { get; set; }

        [Required]
        public int Number { get; set; }
    }
}
