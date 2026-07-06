using System.ComponentModel.DataAnnotations;

namespace instagram.DTO
{
    public class registerDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
