using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DB.Moduls
{
    public class Post
    {
        [Key , Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Date_Create { get; set; } = new DateTime();
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public List<PostImages> PostImages { get; set; } = new List<PostImages>();
        public List<CommandPost> CommandPosts { get; set; } = new List<CommandPost>();
        public List<LikePost> LikePosts { get; set; } = new List<LikePost>();
        public List<LikeCommand> LikeCommands { get; set; } = new List<LikeCommand>();
    }
}
