namespace instagram.DTO.PostFull
{
    public class GetCommentPost
    {
       public int Id { get; set; }
        public string Comment { get; set; }

        public string userName { get; set; }
        public string ImageProfile { get; set; }
        public List<GetLikeToCommentDto> CommentLikes { get; set; } = new List<GetLikeToCommentDto>();

    }
}
