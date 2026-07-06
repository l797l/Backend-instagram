using System.ComponentModel.DataAnnotations;

namespace instagram.DTO
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}
