using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DB.Moduls
{
    public class Follow
    {
        [Key,Required]
        public int Id { get; set; }
        [Required] public string FollowerId { get;set; }
        [Required]
        public string FollowingId { get; set; }

        [ForeignKey(nameof(FollowerId))]
        public User Follower { get; set; }
        [ForeignKey(nameof(FollowingId))]
        public User Following { get; set; }
    }
}
