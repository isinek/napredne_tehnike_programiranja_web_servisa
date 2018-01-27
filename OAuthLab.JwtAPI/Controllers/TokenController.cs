using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using OAuthLab.DAL.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using OAuthLab.JwtAPI.Helpers;
using Microsoft.Extensions.Configuration;
using OAuthLab.JwtAPI.Models;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OAuthLab.JwtAPI.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/Token")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        private readonly StoreSampleContext _context;

        public IConfiguration Configuration { get; }

        public TokenController(StoreSampleContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpPost("RequestToken")]
        public async Task<IActionResult> RequestToken([FromBody] TokenRequestModel tokenRequest)
        {
            if (tokenRequest != null &&
                await _context.Customer.AnyAsync(c => c.FirstName == tokenRequest.FirstName
                                                    && c.Phone == tokenRequest.Phone))
            {
                JwtSecurityToken token = JwsTokenCreator.CreateToken(tokenRequest.FirstName,
                                                                    Configuration["Auth:JwtSecurityKey"],
                                                                    Configuration["Auth:ValidIssuer"],
                                                                    Configuration["Auth:ValidAudience"]);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenStr);
            }
            return Unauthorized();
        }

        [HttpGet("RequestTokenVersion")]
        [HttpPost("RequestTokenVersion")]
        [MapToApiVersion("1.0"), MapToApiVersion("1.1")]
        public string GetApiVersion() => HttpContext.GetRequestedApiVersion().ToString();
    }

    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("api/Token")]
    [AllowAnonymous]
    public class TokenV1_1Controller : Controller
    {
        private readonly StoreSampleContext _context;

        public IConfiguration Configuration { get; }

        public TokenV1_1Controller(StoreSampleContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpPost("RequestToken")]
        public async Task<IActionResult> RequestToken([FromBody] TokenRequestModel tokenRequest)
        {
            var user = await _context.Customer.FirstOrDefaultAsync(c => c.SecretHash == tokenRequest.SecretHash);
            if (user != null)
            {
                JwtSecurityToken token = JwsTokenCreator.CreateToken(tokenRequest.SecretHash,
                                                                    Configuration["Auth:JwtSecurityKey"],
                                                                    Configuration["Auth:ValidIssuer"],
                                                                    Configuration["Auth:ValidAudience"]);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenStr);
            }
            return Unauthorized();
        }
    }
}