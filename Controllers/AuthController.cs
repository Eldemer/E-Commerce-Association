using Authontcation_Test.DTO;
using Authontcation_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authontcation_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplecationUser> userManager;
        private readonly IConfiguration configuration;
        public AuthController(UserManager<ApplecationUser> _userManger, IConfiguration _configuration)
        {
            userManager = _userManger;
            configuration = _configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            ApplecationUser User = new ApplecationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName

            };
            var emailcheek = await userManager.FindByEmailAsync(model.Email);
            if (emailcheek != null)
            {
                return BadRequest("Email Is Alrady Used ");
            }

            //var CheckFullName = await userManager.Users.AnyAsync(u=>u.FullName==model.FullName);
            //if (CheckFullName != null)
            //{
            //    return BadRequest("Full Name Is Alrady Used");
            //}

            var result = await userManager.CreateAsync(User, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User Register Succesfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var User = await userManager.FindByEmailAsync(model.EmailOrUserName);
            if (User == null)
            {
                User = await userManager.FindByNameAsync(model.EmailOrUserName);
            }

            if (User == null || !await userManager.CheckPasswordAsync(User, model.Password))
            {
                return Unauthorized("Email Or Password Not Correct");
            }

            //Generate Token

            var token = await GenerateJwtToken(User);

            return Ok(new { token });

        }

        private async Task<string> GenerateJwtToken(ApplecationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: null,
               claims: claims,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        [Authorize]
        [HttpPost("LogOut")]
        public IActionResult LogOut()
        {
            return Ok("Loged out Successfuly");
        }

    }
}