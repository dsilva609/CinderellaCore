using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.TheMovieDatabase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CinderellaCore.Services.Features.Movie
{
    public class TMDBService : ITMDBService
    {
        private readonly GlobalSettings _settings;
        private HttpClient _client;

        public TMDBService(GlobalSettings settings)
        {
            _settings = settings;
            CreateClient();
        }

        public List<TMDBMovie> SearchMovies(string title)
        {
            var response = _client.GetAsync($"search/movie?api_key={_settings.TMDBKey}&query={title}");

            var result = response.Result.Content.ReadAsStringAsync().Result;
            var movies = JsonConvert.DeserializeObject<TMDBMovieResult>(result);

            return movies.results;
        }

        public Model.Models.Movie SearchMovieByID(int id)
        {
            var response = _client.GetAsync($"movie/{id}?api_key={_settings.TMDBKey}");

            var result = response.Result.Content.ReadAsStringAsync().Result;
            var tmdbMovie = JsonConvert.DeserializeObject<TMDBMovie>(result);
            var movie = ConvertTMDDResultToModelForMovie(tmdbMovie);

            return movie;
        }

        public List<TMDBMovie> SearchTV(string title)
        {
            var response = _client.GetAsync($"search/tv?api_key={_settings.TMDBKey}&query={title}");

            var result = response.Result.Content.ReadAsStringAsync().Result;
            var shows = JsonConvert.DeserializeObject<TMDBMovieResult>(result);
            shows.results.ForEach(x => x.IsTvShow = true);

            return shows.results;
        }

        public Model.Models.Movie SearchTVShowByID(int id, int seasonNumber)
        {
            var response = _client.GetAsync($"tv/{id}?api_key={_settings.TMDBKey}");

            var result = response.Result.Content.ReadAsStringAsync().Result;
            var tmdbMovie = JsonConvert.DeserializeObject<TMDBMovie>(result);
            var movie = ConvertTMDDResultToModelForTV(tmdbMovie, seasonNumber);

            return movie;
        }

        private void CreateClient()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://api.themoviedb.org/3/") };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("User-Agent", "Project-Cinderella/1.0 +projectcinderella.azurewebsites.net");
        }

        private Model.Models.Movie ConvertTMDDResultToModelForMovie(TMDBMovie tmdb)
        {
            var movie = new Model.Models.Movie
            {
                Title = tmdb.title,
                Distributor = tmdb.production_companies.First().name,
                Genre = string.Join(", ", tmdb.genres.Select(x => x.name).ToList()),
                ImageUrl = string.Format("https://image.tmdb.org/t/p/w500{0}", tmdb.poster_path),
                Language = tmdb.original_language,
                TMDBID = tmdb.id,
                YearReleased = DateTime.Parse(tmdb.release_date).Year,
                SeasonNumber = 0
            };

            return movie;
        }

        private Model.Models.Movie ConvertTMDDResultToModelForTV(TMDBMovie tmdb, int seasonNumber)
        {
            var movie = new Model.Models.Movie
            {
                Title = tmdb.name,
                Director = string.Join(", ", tmdb.created_by.Select(x => x.name).ToList()),
                Distributor = tmdb.production_companies?.FirstOrDefault()?.name,
                Genre = string.Join(", ", tmdb.genres.Select(x => x.name).ToList()),
                ImageUrl = string.Format("https://image.tmdb.org/t/p/w500{0}", tmdb.poster_path),
                Language = tmdb.original_language,
                TMDBID = tmdb.id,
                YearReleased = DateTime.Parse(tmdb.first_air_date).Year,
            };

            if (seasonNumber > 0 && tmdb.seasons.SingleOrDefault(x => x.season_number == seasonNumber) != null)
            {
                var season = tmdb.seasons.SingleOrDefault(x => x.season_number == seasonNumber);
                movie.Title += $" Season {seasonNumber}";
                movie.SeasonNumber = seasonNumber;
                movie.TMDBID = season.id;
                movie.YearReleased = Convert.ToDateTime(season.air_date).Year;
                movie.ImageUrl = string.Format("https://image.tmdb.org/t/p/w500{0}", season.poster_path);
            }

            return movie;
        }
    }
}