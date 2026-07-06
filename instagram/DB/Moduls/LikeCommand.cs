using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DB.Moduls
{
    public class LikeCommand
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CommandId { get; set; }
        [Required]
        public int PostId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("CommandId")]
        public CommandPost Command { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
