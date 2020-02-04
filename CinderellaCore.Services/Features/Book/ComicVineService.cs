using CinderellaCore.Model.Enums;
using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.ComicVine;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Features.Book
{
    public class ComicVineService : IComicVineService
    {
        private readonly GlobalSettings _settings;
        private HttpClient _client;

        public ComicVineService(GlobalSettings settings)
        {
            _settings = settings;
            CreateClient();
        }

        public async Task<ComicVineResult> Search(string query)
        {
            var response = await _client.GetStringAsync($"search/?api_key={_settings.ComicVineKey}&resources=issue&format=json&limit=25&query={query}");

            var comicVineResults = JsonConvert.DeserializeObject<ComicVineResult>(response);
            comicVineResults.results.ForEach(x => x.id = x.api_detail_url.Substring(x.api_detail_url.IndexOf("issue/") + 6).TrimEnd('/'));
            return comicVineResults;
        }

        public async Task<Model.Models.Book> SearchByID(string id)
        {
            var response = await _client.GetStringAsync($"issue/{id}/?api_key={_settings.ComicVineKey}&format=json");

            var comicVineResult = JsonConvert.DeserializeObject<ComicVineComic>(response);
            var book = ConvertFromComicVineResultToBook(comicVineResult);

            return book;
        }

        private Model.Models.Book ConvertFromComicVineResultToBook(ComicVineComic result)
        {
            var book = new Model.Models.Book();
            var comic = result.results;
            //TODO: add field for ComicVineID

            book.Title = $"{comic.name} #{comic.issue_number}";
            book.ImageUrl = comic.image.super_url;
            if (!string.IsNullOrWhiteSpace(book.ImageUrl) && !book.ImageUrl.Contains("https")) book.ImageUrl = book.ImageUrl.Replace("http", "https");
            book.GoogleBookID = comic.api_detail_url.Substring(comic.api_detail_url.IndexOf("issue/") + 6).TrimEnd('/');
            book.Author = comic.person_credits?.FirstOrDefault(x => x.role == "writer")?.name;
            book.Publisher = comic.publisher?.name;
            book.Type = BookTypeEnum.Comic;

            return book;
        }

        private void CreateClient()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://api.comicvine.com/") };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("User-Agent", "Project-Cinderella/1.0 +projectcinderella.azurewebsites.net");
        }
    }
}