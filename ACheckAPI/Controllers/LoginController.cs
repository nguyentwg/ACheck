using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ACheckAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ACheckAPI.Controllers
{
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        //[HttpPost]
        [HttpPost("authenticate")]
        public IActionResult Login()
        {
            IActionResult response = Unauthorized();
            var tokenString = GenerateJSONWebToken();
            response = Ok(new { token = tokenString });
            return response;
        }

        private string GenerateJSONWebToken()
        {
            var Key = AppSetting.Key;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //var claims = new[] {
            //    new Claim(JwtRegisteredClaimNames.Sub, "Admin"),
            //    new Claim(JwtRegisteredClaimNames.Email, "Admin"),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};
            var jwt = new JwtSecurityToken(
                claims: null,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: credentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            //var token = new JwtSecurityToken(Site,
            //  null,
            //  expires: DateTime.Now.AddMinutes(120),
            //  signingCredentials: credentials);
            return encodedJwt;
        }
    }
}