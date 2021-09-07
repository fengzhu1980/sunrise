using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.DataModels.Models;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthenticationConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public TokenService(AuthenticationConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        }

        public Task AddRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid().ToString();
            _refreshTokens.Add(refreshToken);

            return Task.CompletedTask;
        }

        public ReturnTokenModel CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };

            return GenerateToken(claims);
        }

        private ReturnTokenModel GenerateToken(List<Claim> claims = null)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.ExpirationMinutes),
                SigningCredentials = creds,
                Issuer = _config.Issuer
            };

            if (claims != null) tokenDescriptor.Subject = new ClaimsIdentity(claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var returnToken = tokenHandler.WriteToken(token);
            var result = new ReturnTokenModel()
            {
                Token = returnToken,
                ExpiresIn = _config.ExpirationMinutes
            };

            return result;
        }

        public ReturnTokenModel GenerateRefreshToken()
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.RefreshTokenExpirationMinutes),
                SigningCredentials = creds,
                Issuer = _config.Issuer
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var returnToken = tokenHandler.WriteToken(token);
            var result = new ReturnTokenModel()
            {
                Token = returnToken,
                ExpiresIn = _config.RefreshTokenExpirationMinutes
            };

            return result;
        }

        public Task<RefreshToken> GetRefreshTokenByToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(r => r.Token == token);
            return Task.FromResult(refreshToken);
        }

        public bool Validate(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = _key,
                ValidIssuer = _config.Issuer,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key
            }, out SecurityToken validatedToken);
        }

        public Task DeleteRefreshTokenById(string id)
        {
            _refreshTokens.RemoveAll(r => r.Id == id);
            return Task.CompletedTask;
        }

        public Task DeleteRefreshTokenByUserId(string userId)
        {
            _refreshTokens.RemoveAll(r => r.UserId == userId);
            return Task.CompletedTask;
        }
    }
}