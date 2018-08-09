using System.Linq;
using CinderellaCore.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CinderellaCore.Web.Controllers.Api
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

        [Route("GetShowcasedItems/{id}")]
        [Authorize(Policy = "Api")]
        [HttpGet]
        public IActionResult GetShowcasedItems(int id)
        {
            var albums = _albumService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList();

            return Ok(JsonConvert.SerializeObject(albums));
        }
    }
}