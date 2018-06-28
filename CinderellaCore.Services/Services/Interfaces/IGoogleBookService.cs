using System;
namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IGoogleBookService
    {
        List<Book> Search(string author, string title);

        Book SearchByID(string id);
    }
}
