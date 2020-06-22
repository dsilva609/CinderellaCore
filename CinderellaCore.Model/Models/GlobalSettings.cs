using System;

namespace CinderellaCore.Model.Models
{
    public class GlobalSettings
    {
        public string DiscogsKey { get; set; }
        public string ComicVineKey { get; set; }
        public string GiantBombKey { get; set; }
        public string TMDBKey { get; set; }
        public DateTime RecordStoreDayDate { get; set; }
        public DateTime FreeComicBookDayDate { get; set; }
        public string ApiKey { get; set; }
        public string JwtKey { get; set; }
        public string Issuer { get; set; }
    }
}