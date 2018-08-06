using CinderellaCore.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace CinderellaCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowcaseController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public ShowcaseController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [Route("getshowcaseditems/{id}")]
        [HttpGet]
        public IActionResult GetShowcasedItems(int id)
        {
            var albums = _albumService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList();

            return Ok(JsonConvert.SerializeObject(albums));
        }
    }
}