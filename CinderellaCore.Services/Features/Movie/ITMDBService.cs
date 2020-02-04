using CinderellaCore.Model.Models.TheMovieDatabase;
using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Movie
{
    public interface ITMDBService
    {
        List<TMDBMovie> SearchMovies(string title);

        Model.Models.Movie SearchMovieByID(int id);

        List<TMDBMovie> SearchTV(string title);

        Model.Models.Movie SearchTVShowByID(int id, int seasonNumber);
    }
}