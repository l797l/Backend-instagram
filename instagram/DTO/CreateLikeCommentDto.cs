using System.ComponentModel.DataAnnotations;

namespace instagram.DTO
{
    public class CreateLikeCommentDto
    {
        [Required]
        public int CommandId { get; set; }
    }
}
