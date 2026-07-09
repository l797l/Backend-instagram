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
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
