using Microsoft.AspNetCore.Identity;

namespace Authontcation_Test.Models
{
    public class ApplecationUser:IdentityUser
    {
        public string  FullName { get; set; }
    }
}
