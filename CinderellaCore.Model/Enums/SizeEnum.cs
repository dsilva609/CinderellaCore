﻿using System.ComponentModel.DataAnnotations;

namespace CinderellaCore.Model.Enums
{
    public enum SizeEnum
    {
        [Display(Name = "12\"")]
        Twelve,

        [Display(Name = "7\"")]
        Seven,

        [Display(Name = "10\"")]
        Ten,

        Other
    }
}