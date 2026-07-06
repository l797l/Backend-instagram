using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DB.Moduls
{
    public class PostImages

    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Link_Image { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
