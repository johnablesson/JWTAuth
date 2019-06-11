using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTAuth.Controllers
{
    [ApiController]

    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("Account/Login")]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult result = Unauthorized();

            var user = AuthenticateUser(login);

            if (user != null)
            {
                var token = GenerateJWT(login.UserName);
                result = Ok(new { access_token = token, refresh_token = GenerateRefreshToken() });
            }

            return result;
        }

        private string GenerateJWT(string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("passlkjdf dlskfjlsdkfjlsd flsdjflksd flksd fj sdjfl ksdjfkl sdjf ksdj lfsdj fk jsld fjsld kfj sldk fsd flword"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim("Role", "Manager")
            };

            var token = new JwtSecurityToken(
                                                issuer: "Calpion",
                                                audience: "TruckstopMobileFrontEnd",
                                                claims: claims,
                                                notBefore: DateTime.Now,
                                                expires: DateTime.Now.Add(TimeSpan.FromMinutes(1)),
                                                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [HttpPost]
        [Route("Account/Refresh")]
        public IActionResult Refresh(RefreshTokenModel refreshTokenModel)
        {
            var token = GenerateJWT(refreshTokenModel.RefreshToken);
           return Ok(new { access_token = token, refresh_token = GenerateRefreshToken() });
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            if (login.UserName == "John" && login.Password == "p")
            {
                user = new UserModel() { UserName = "John", Email = "johnablesson@truckstop.com" };
            }

            return user;
        }
    }
}