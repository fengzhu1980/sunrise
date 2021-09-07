using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.DataModels.Models;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IGenericRepository<Staff> staffRepo
        ) : base(userManager, staffRepo)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<UserDto> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            var tempToken = _tokenService.CreateToken(user);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = tempToken.Token,
                ExpiresIn = tempToken.ExpiresIn,
                Email = user.Email,
                UserId = user.Id
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            var tempToken = _tokenService.CreateToken(user);
            var returnValue = new UserDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = tempToken.Token,
                ExpiresIn = tempToken.ExpiresIn,
                RefreshToken = _tokenService.GenerateRefreshToken().Token,
                DisplayName = user.DisplayName
            };

            RefreshToken refreshToken = new RefreshToken()
            {
                Token = returnValue.RefreshToken,
                UserId = user.Id
            };

            await _tokenService.AddRefreshToken(refreshToken);

            return returnValue;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use"}});
            }
            
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var tempToken = _tokenService.CreateToken(user);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = tempToken.Token,
                ExpiresIn = tempToken.ExpiresIn,
                Email = user.Email
            };
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthenticatedUserResponse>> Refresh(RefreshToken refreshToken)
        {
            if (refreshToken.Token == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Refresh token can not be null"}});
            }
            else if (refreshToken.UserId == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"User id can not be null"}});
            }
            else
            {
                bool isValidRefreshToken = _tokenService.Validate(refreshToken.Token);
                if (!isValidRefreshToken)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Invalid refresh token"}});
                }

                // RefreshToken refreshTokenDTO = await _tokenService.GetRefreshTokenByToken(refreshToken.Token);
                // if (refreshTokenDTO == null)
                // {
                //     return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Invalid refresh token"}});
                // }

                var user = await _userManager.FindByIdAsync(refreshToken.UserId);
                
                var accessToken = _tokenService.CreateToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                RefreshToken refreshTokenNew = new RefreshToken()
                {
                    Token = newRefreshToken.Token,
                    UserId = user.Id
                };

                // Delete old refresh token
                // await _tokenService.DeleteRefreshTokenById(refreshTokenDTO.Id);

                // Add new refresh token
                // await _tokenService.AddRefreshToken(refreshTokenNew);

                AuthenticatedUserResponse response = new AuthenticatedUserResponse()
                {
                    AccessToken = accessToken.Token,
                    RefreshToken = newRefreshToken.Token,
                    ExpiresIn = accessToken.ExpiresIn
                };

                return response;
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var user = await GetCurrentUser();

            // Delete old refresh token
            await _tokenService.DeleteRefreshTokenByUserId(user.UserId);

            return Ok();
        }

        // [Authorize]
        // [HttpPut("updateuser")]
        // public async Task<ActionResult<UserDto>> UpdateUser(UserDto user)
        // {
        //     var userFound = await _userManager.FindByEmailAsync(user.Email);
        //     if (user == null) return BadRequest(new ApiResponse(400, "User not found"));
        //     user.DisplayName = user.DisplayName;

        //     var reuslt = await _userManager.UpdateAsync(userFound);
        //     if (!reuslt.Succeeded) return Ok(user);

        //     return BadRequest("Problem updating the user");
        // }
    }
}