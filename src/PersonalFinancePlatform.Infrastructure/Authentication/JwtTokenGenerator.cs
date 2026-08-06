using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalFinancePlatform.Application.Interfaces.Authentication;
using PersonalFinancePlatform.Application.Record.Authentication;
using PersonalFinancePlatform.Domain.User.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Authentication
{
    public sealed class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions = options.Value;

        public JwtToken Generate(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var expiresAt = now.AddMinutes(_jwtOptions.ExpirationMinutes);
         
            //Build identity claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            //Create signing credentials
            var secretBytes = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                notBefore: now,
                claims: claims,
                expires: expiresAt,
                signingCredentials: signingCredentials
                );


            //Serialize JWT
            var accessToken = handler.WriteToken(token);
            return new JwtToken(accessToken, expiresAt);
        }
    }
}
