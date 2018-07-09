using CinderellaCore.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;

namespace CinderellaCore.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImportController : CinderellaCoreBaseController
	{
		public ImportController(UserManager<ApplicationUser> userManager) : base(userManager)
		{
		}

		[Authorize(Policy = "Import")]
		[Route("ImportAlbums")]
		[HttpPost]
		public IActionResult ImportAlbums(HttpRequestMessage request)
		{
			if (!_user.EnableImport) return BadRequest("User Import not enabled");
			throw new NotImplementedException();
		}

		[Route("Test")]
		[HttpGet]
		public IActionResult Test()
		{
			return Ok("Whaaaaa");
		}
	}
}