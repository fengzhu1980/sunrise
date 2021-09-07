using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.DataModels.Models;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        ReturnTokenModel CreateToken(AppUser user);
        ReturnTokenModel GenerateRefreshToken();
        bool Validate(string refreshToken);
        Task AddRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByToken(string token);
        Task DeleteRefreshTokenById(string id);
        Task DeleteRefreshTokenByUserId(string userId);
    }
}