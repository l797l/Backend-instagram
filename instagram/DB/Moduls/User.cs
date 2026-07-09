using Microsoft.AspNetCore.Identity;

namespace instagram.DB.Moduls
{
    public class User : IdentityUser
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Data_Time_Create { get; set; } = DateTime.Now;
        public string? Link_Image_Profile { get; set; } = null;
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<CommandPost> CommandPosts { get; set; } = new List<CommandPost>();
        public List<LikePost> LikePosts { get; set; } = new List<LikePost>();
        public List<LikeCommand> LikeCommands { get; set; } = new List<LikeCommand>();
        public ICollection<Follow> Followers { get; set; }
        public ICollection<Follow> Following { get; set; }
    }
}
