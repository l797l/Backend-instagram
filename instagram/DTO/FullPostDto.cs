using instagram.DB.Moduls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace instagram.DTO
{
    public class FullPostDto
    {

        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Date_Create { get; set; } = new DateTime();
        [Required]
        public string UserId { get; set; }
        public string Link_Image { get; set; }
        public int countLikeToPost { get; set; }
        public int countCommentToPost { get; set; }




    }
}
