using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using EcommerceSportsShopNetAndAngular.Controllers;
using EcommerceSportsShopNetAndAngular.Dtos;
using EcommerceSportsShopNetAndAngular.Errors;
using EcommerceSportsShopNetAndAngular.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> UserManager;
        private readonly SignInManager<AppUser> SignInManager;
        private readonly ITokenService TokenService;
        private readonly IMapper Mapper;
        public readonly IUsersRepository<AppUser> UsersRepository;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            Mapper = mapper;
            TokenService = tokenService;
            SignInManager = signInManager;
            UserManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await UserManager.FindByEmailFromClaimsPrincipal(User);

            return new UserDto
            {
                Email = user.Email,
                Token = TokenService.GenerateAccessToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await UserManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await SignInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = TokenService.GenerateAccessToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                { Errors = new[] { "Email address is in use" } });
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await UserManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = TokenService.GenerateAccessToken(user),
                Email = user.Email
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await UserManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await UserManager.FindUserByClaimsPrincipleWithAddress(User);

            return Mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await UserManager.FindUserByClaimsPrincipleWithAddress(User);

            user.Address = Mapper.Map<AddressDto, Address>(address);

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(Mapper.Map<AddressDto>(user.Address));

            return BadRequest("Problem updating the user");
        }
    }
}