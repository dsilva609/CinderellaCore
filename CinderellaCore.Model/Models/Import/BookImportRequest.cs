using System.Collections.Generic;

namespace CinderellaCore.Model.Models.Import
{
	public class BookImportRequest : BaseImportRequest
	{
		public List<Book> Books { get; set; }
	}
}