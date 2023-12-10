using Manager.Core;
using Manager.Models;
using Manager.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Controllers
{
    [Route("api/[controller]")]
    public class Authentication : ControllerBase
    {
        private readonly ManagerDBContext _dbContext;
        private readonly IConfiguration _configuration;
        public string hash;
        public Authentication(ManagerDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            var userLogin = _dbContext.Vendedores.SingleOrDefault(v => v.UserName == user.UserName && v.Pass == user.Pass);
            if(userLogin == null)
            {
                return NotFound();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Strings").GetSection("Hash").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                    //new Claim("Store", "user")
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var userView = new UserView(userLogin.Nome, token, userLogin.Id);

            return Ok(userView);
        }
    }
}
