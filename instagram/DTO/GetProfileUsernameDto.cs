using instagram.DB.Moduls;

namespace instagram.DTO
{
    public class GetProfileUsernameDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ImagProfile { get; set; }
        public List<GetPostProfileDto> Posts { get; set; } = [];

        public List<string> Followers { get; set; } = [];
        public List<string> Folloings { get; set; } = [];




    }
}
