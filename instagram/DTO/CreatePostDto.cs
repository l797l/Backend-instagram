namespace instagram.DTO
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }
}
