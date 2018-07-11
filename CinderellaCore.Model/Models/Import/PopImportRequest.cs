using System.Collections.Generic;

namespace CinderellaCore.Model.Models.Import
{
    public class PopImportRequest : BaseImportRequest
    {
        public List<FunkoPop> Pops { get; set; }
    }
}