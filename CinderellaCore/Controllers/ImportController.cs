﻿using CinderellaCore.Model.Models;
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

        [Authorize(Policy = "Import")]
        [Route("ImportAlbums")]
        [HttpPost]
        public async Task<IActionResult> ImportAlbums([FromBody]AlbumImportRequest request)
        {
            if (request == null) return BadRequest("Request missing");

            var result = await _importService.ImportAlbumAsync(request);

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