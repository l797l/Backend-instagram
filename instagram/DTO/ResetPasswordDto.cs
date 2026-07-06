using System.ComponentModel.DataAnnotations;

namespace instagram.DTO
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
       public string newPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
