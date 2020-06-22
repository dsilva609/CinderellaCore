using CinderellaCore.Model.Models;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CinderellaCore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly GlobalSettings _settings;

        public LoginController(GlobalSettings settings) => _settings = settings;

        [HttpGet("Login")]
        public IActionResult Login(string username, string password)
        {
            LoginModel login = new LoginModel { Email = username, Password = password };

            IActionResult response = Unauthorized();

            var userResult = AuthenticateUser(login);

            if (userResult != null)
            {
                string tokenString = GenerateToken(userResult);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        [Route("TestPost")]
        [Produces("application/json")]
        [HttpPost]
        [Authorize]
        public IActionResult TestPost()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            string username = identity.Claims.ToList().FirstOrDefault().Value;

            return Ok($"Hello {username}");
        }

        [HttpGet("TestGet")]
        [Authorize]
        public ActionResult<string> TestGet() => Ok("Authorized doot");

        private string GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Issuer,
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        private ApplicationUser AuthenticateUser(LoginModel login)
        {
            ApplicationUser user = null;

            if (login.Email == "dsilva609@gmail.com" && login.Password == "test")
            {
                user = new ApplicationUser { Email = "dsilva609@gmail.com", UserName = "Holy Diver" };
            }

            return user;
        }
    }
}