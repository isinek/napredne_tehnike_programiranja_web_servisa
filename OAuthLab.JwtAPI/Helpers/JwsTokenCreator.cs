﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuthLab.JwtAPI.Helpers
{
    public static class JwsTokenCreator
    {
        public static JwtSecurityToken CreateToken(string firstName, string secret, string validIssuer, string validAudience)
        {
            var claims = new Claim[]
                            {
                                new Claim(ClaimTypes.Name, firstName)
                            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                    issuer: validIssuer,
                    audience: validAudience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: creds);
        }
    }
}
