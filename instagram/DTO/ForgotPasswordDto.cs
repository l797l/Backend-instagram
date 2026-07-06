using System.ComponentModel.DataAnnotations;

namespace instagram.DTO
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
