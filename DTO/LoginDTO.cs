using System.ComponentModel.DataAnnotations;

namespace Authontcation_Test.DTO
{
    public class LoginDTO
    {
        [Required]
        public string EmailOrUserName { get; set; }
       
        [Required]
        public string Password { get; set; }

    }
}
