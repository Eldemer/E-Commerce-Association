using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authontcation_Test.Models
{
    public class ApplecationDbContext:IdentityDbContext<ApplecationUser>
    {
        public ApplecationDbContext(DbContextOptions<ApplecationDbContext> options):base(options)
        {            
        }
        public DbSet<Home> Home { get; set; }
    }
}
