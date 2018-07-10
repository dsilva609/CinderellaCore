using CinderellaCore.Model.Models.BoardGameGeek;
using CinderellaCore.Model.Models.GiantBomb;

namespace CinderellaCore.Web.Models
{
    public class GameSearchModel
    {
        public string Title { get; set; }

        public GiantBombResult GiantBombResult { get; set; }

        public BGGGame BGGResult { get; set; }
    }
}