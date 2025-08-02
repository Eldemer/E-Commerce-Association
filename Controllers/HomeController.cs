using Authontcation_Test.DTO;
using Authontcation_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authontcation_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplecationDbContext _context;
        public HomeController(ApplecationDbContext context)
        {
            _context = context;
        }

        [HttpGet] //https://localhost:7159/api/Home
        public async Task<IActionResult> GetData()
        {
            Home home =await _context.Home.FirstOrDefaultAsync();
            if(home==null)
            {
                return NotFound("No data found.");
            }
            HomeDTO homeDTO = new HomeDTO
            {
                MainTitle=home.MainTitle,
                AboutUs=home.AboutUs,
                Definition=home.Definition,
                SponsorsTitle=home.SponsorsTitle
            };

            return Ok(homeDTO);   
        }




    }
}
