using Authontcation_Test.DTO;
using Authontcation_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Authontcation_Test.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplecationUser> _userManager;
        public ProfileController(UserManager<ApplecationUser> usermanger)
        {
            _userManager = usermanger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileData()
        {
            var userid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userid == null)
            {
                return Unauthorized("Invalid Token or User not found.");
            }
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound("User not found.");  
            }
            
            ProfileDTO profileDTO = new ProfileDTO
            {
                Id=user.Id,
                FullName=user.FullName,
                Email=user.Email,
                UserName = user.UserName
            };

            return Ok(profileDTO);  
        }
    }
}
