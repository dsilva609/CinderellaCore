using System;
using System.ComponentModel.DataAnnotations;

namespace CinderellaCore.Model.Enums
{
    public enum MovieMediaTypeEnum
    {
        DVD,

        [Display(Name = "Blu-ray")]
        Bluray
    }
}
