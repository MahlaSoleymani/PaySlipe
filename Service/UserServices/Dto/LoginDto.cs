
using System.ComponentModel.DataAnnotations;

namespace Service.UserServices.Dto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
