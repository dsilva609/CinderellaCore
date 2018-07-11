using System.Collections.Generic;

namespace CinderellaCore.Model.Models.Import
{
    public class GameImportRequest : BaseImportRequest
    {
        public List<Game> Games { get; set; }
    }
}