using System.ComponentModel.DataAnnotations;

namespace instagram.DTO
{
    public class loginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
