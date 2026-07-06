using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DB.Moduls
{
    public class LikePost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
