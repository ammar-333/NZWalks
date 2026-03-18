using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;

        }

        //POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName
            };

            var IdentityResult = await userManager.CreateAsync(identityUser, registerDto.Password);

            if (!IdentityResult.Succeeded)
            {
                return BadRequest(IdentityResult.Errors);
            }
            
             // add roles
             if(registerDto.Roles != null && registerDto.Roles.Any())
             {
                  IdentityResult = await userManager.AddToRolesAsync(identityUser, registerDto.Roles);

                  if (!IdentityResult.Succeeded)
                  {
                      return BadRequest(IdentityResult.Errors);
                  }
             }
            
            return Ok("User Was registered");
        }


        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.UserName);

            if(user == null)
            {
                return BadRequest("invalid email or password");
            }

            var checkPassword = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!checkPassword)
            {
                return BadRequest("invalid email or password");
            }

            var roles = await userManager.GetRolesAsync(user);

            if(roles == null)
            {
                return BadRequest("invalid email or password");
            }

            var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

            var respone = new LoginResponeDto
            {
                JwtToken = jwtToken
            };

            return Ok(respone);
        }



    }
}
