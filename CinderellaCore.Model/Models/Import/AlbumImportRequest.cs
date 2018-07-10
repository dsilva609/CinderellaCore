using System.Collections.Generic;

namespace CinderellaCore.Model.Models.Import
{
    public class AlbumImportRequest : BaseImportRequest
    {
        public List<Album> Albums { get; set; }
    }
}