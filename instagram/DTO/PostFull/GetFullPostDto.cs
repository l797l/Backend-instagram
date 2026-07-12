namespace instagram.DTO.PostFull
{
    public class GetFullPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<GetImagePostDto> Images { get; set; } = new List<GetImagePostDto>();
        public List<GetLikePostDto> LikePosts { get; set; } = new List<GetLikePostDto>();
        public List<GetCommentPost> CommentPosts { get; set; } = new List<GetCommentPost>();
        public GetUserPostDto User { get; set; }
    }
}
