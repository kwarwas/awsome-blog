using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AwesomeBlog.Api.ViewModels;
using AwesomeBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AwesomeBlog.Api.Controllers
{
    public class JwtSettings
    {
        public string ValidIssuer { get; set; } = "https://localhost:5001";
        public string ValidAudience { get; set; } = "https://localhost:5001";
        public string Secret { get; set; } = "2750F013-7ED3-4E03-BA78-B0A0604F5482";
        public int LifetimeInSeconds { get; set; } = 3600;
    }

    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateUserViewModel createUserViewModel)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(createUserViewModel.Password);
            
            return Ok(new
            {
                pass = hashed,
                ver = BCrypt.Net.BCrypt.Verify(createUserViewModel.Password, "$2b$10$vnoE2kgbxZq8ArR4sJvnl.fel9mit2/1qoUpIpJyYXrv3zW8BVkaC")
            });
        }

        [HttpPost("login")]
        public ActionResult Login([FromForm] LoginViewModel loginViewModel)
        {
            if (loginViewModel.Password != "password")
            {
                return BadRequest();
            }

            var jwtSettings = new JwtSettings();

            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginViewModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "User"),
                }),
                Expires = DateTime.UtcNow.AddSeconds(jwtSettings.LifetimeInSeconds),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = jwtSettings.ValidIssuer,
                Audience = loginViewModel.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return Ok
            (
                new {access_token = tokenHandler.WriteToken(token)}
            );
        }
    }
}