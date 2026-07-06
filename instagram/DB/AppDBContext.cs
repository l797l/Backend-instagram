using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using instagram.DB.Moduls;
using Microsoft.EntityFrameworkCore;
namespace instagram.DB
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<CommandPost> CommandPost { get; set; }
        public DbSet<LikePost> LikePost { get; set; }
        public DbSet<LikeCommand> LikeCommand { get; set; }
        public DbSet<PostImages> PostImages { get; set; }
        public DbSet<EmailOtp> EmailOtps { get; set; }


    }
}
