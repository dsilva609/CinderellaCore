using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Import;
using CinderellaCore.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinderellaCore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : CinderellaCoreBaseController
    {
        private readonly IImportService _importService;

        public ImportController(UserManager<ApplicationUser> userManager, IImportService importService) : base(userManager)
        {
            _importService = importService;
        }

        [Authorize(Policy = "Api")]
        [Route("ImportAlbums")]
        [HttpPost]
        public async Task<IActionResult> ImportAlbums([FromBody] AlbumImportRequest request)
        {
            if (request == null) return BadRequest("Request missing");

            var result = await _importService.ImportAlbumsAsync(request);

            return Json(result);
        }

        [Authorize(Policy = "Api")]
        [Route("ImportBooks")]
        [HttpPost]
        public async Task<IActionResult> ImportBooks([FromBody] BookImportRequest request)
        {
            if (request == null) return BadRequest("Request missing");

            var result = await _importService.ImportBooksAsync(request);

            return Json(result);
        }

        [Authorize(Policy = "Api")]
        [Route("ImportGames")]
        [HttpPost]
        public async Task<IActionResult> ImportGames([FromBody] GameImportRequest request)
        {
            if (request == null) return BadRequest("Request missing");

            var result = await _importService.ImportGamesAsync(request);

            return Json(result);
        }

        [Authorize(Policy = "Api")]
        [Route("ImportMovies")]
        [HttpPost]
        public async Task<IActionResult> ImportMovies([FromBody] MovieImportRequest request)
        {
            if (request == null) return BadRequest("Request missing");

            var result = await _importService.ImportMoviesAsync(request);

            return Json(result);
        }

        [Authorize(Policy = "Api")]
        [Route("ImportPops")]
        [HttpPost]
        public async Task<IActionResult> ImportPops([FromBody] PopImportRequest request)
        {
            if (request == null) return BadRequest("Request missing");

            var result = await _importService.ImportPopsAsync(request);

            return Json(result);
        }

        [Route("Test")]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Whaaaaa");
        }
    }
}