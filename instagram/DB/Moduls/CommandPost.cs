using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DB.Moduls
{
    public class CommandPost
    {
        [Required , Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public DateTime Date_Create { get; set; } = new DateTime();
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public List<LikeCommand> LikeCommands { get; set; } = new List<LikeCommand>();
    }
}
