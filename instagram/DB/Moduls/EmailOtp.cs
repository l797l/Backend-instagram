using System.ComponentModel.DataAnnotations;

namespace instagram.DB.Moduls
{
    public class EmailOtp
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Otp { get; set; }
        [Required]
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; }

    }
}
