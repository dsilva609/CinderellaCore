using System.ComponentModel.DataAnnotations;

namespace CinderellaCore.Model.Enums
{
    public enum GameTypeEnum
    {
        [Display(Name = "Full Game")]
        FullGame,

        DLC,
        Expansion
    }
}