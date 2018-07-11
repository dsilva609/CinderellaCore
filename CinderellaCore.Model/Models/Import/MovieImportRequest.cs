using System.Collections.Generic;

namespace CinderellaCore.Model.Models.Import
{
    public class MovieImportRequest : BaseImportRequest
    {
        public List<Movie> Movies { get; set; }
    }
}