using System.Collections.Generic;
using System.ComponentModel;

namespace CinderellaCore.Model.Models.Discogs
{
    public class DiscogsResult
    {
        public List<string> Style { get; set; }
        public string Thumb { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public List<string> Format { get; set; }

        [DisplayName("Format")]
        public string FormatString { get; set; }

        public string Uri { get; set; }
        public List<string> Label { get; set; }

        [DisplayName("Label")]
        public string LabelString { get; set; }

        public string Catno { get; set; }
        public int Year { get; set; }
        public List<string> Genre { get; set; }
        public string GenreString { get; set; }
        public string ResourceUrl { get; set; }
        public string Type { get; set; }
        public int ID { get; set; }
        public List<string> Barcode { get; set; }
    }
}