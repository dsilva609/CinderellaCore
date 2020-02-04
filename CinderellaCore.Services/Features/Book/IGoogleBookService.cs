using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Book
{
    public interface IGoogleBookService
    {
        List<Model.Models.Book> Search(string author, string title);

        Model.Models.Book SearchByID(string id);
    }
}