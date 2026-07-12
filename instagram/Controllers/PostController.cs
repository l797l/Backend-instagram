using instagram.DB;
using instagram.DB.Moduls;
using instagram.DTO;
using instagram.DTO.PostFull;
using instagram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace instagram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostController : ControllerBase
    {
        readonly ILogger<PostController> logger;
        readonly AppDBContext appDBContext;
        readonly IImageService imageService;
        readonly IMemoryCache memoryCache;
        public PostController(ILogger<PostController> logger, AppDBContext appDBContext , IImageService imageService , IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.appDBContext = appDBContext;
            this.imageService = imageService;
            this.memoryCache = memoryCache;
        }
        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto postDto)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(UserId))
            {
                return Unauthorized();
            }

            var imageUrl = await imageService.UploadImageAsync(postDto.Image);

            var newPost = new Post
            {
                Title = postDto.Title,
                UserId = UserId,
                PostImages = new List<PostImages>
                {
                    new PostImages
                    {
                        Id = imageUrl.FileId,
                        Link_Image = imageUrl.Url
                    }
                },
            };

            await appDBContext.Posts.AddAsync(newPost);
            await appDBContext.SaveChangesAsync();
            memoryCache.Remove("AllPosts");
            return Created();
        }
        [Authorize]
        [HttpPost("DeletePost/{PostId}")]
        public async Task<IActionResult> DeletePost(int PostId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(UserId)) return Unauthorized();
            var post = await appDBContext.Posts.FirstOrDefaultAsync(x => x.Id == PostId && x.UserId == UserId);
            if (post == null) return NotFound();
            var image = await appDBContext.PostImages.FirstOrDefaultAsync(x => x.PostId == post.Id);
            appDBContext.Posts.Remove(post);
            await appDBContext.SaveChangesAsync();
            await imageService.DeleteImageAsync(image.Id);

            return Ok();
        }

        [Authorize]
        [HttpPost("CreateCommand/{PostId}")]
        public async Task<IActionResult> CreateCommand(int PostId, CreateCommentDto cDto)
        {
            if (!ModelState.IsValid) return BadRequest("Error");
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(UserId)) return Unauthorized();
            var newCommand = new CommandPost
            {
                Content = cDto.Content,
                PostId = PostId,
                UserId = UserId
            };
            await appDBContext.CommandPost.AddAsync(newCommand);
            await appDBContext.SaveChangesAsync();
            return Created();
        }

        [Authorize]
        [HttpPost("DeleteCommand")]
        public async Task<IActionResult> DeleteCommand(int CommentId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(UserId)) return Unauthorized();
            var command = await appDBContext.CommandPost.FirstOrDefaultAsync(x => x.Id == CommentId && x.UserId == UserId);
            if (command == null) return NotFound();
            appDBContext.CommandPost.Remove(command);
            await appDBContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetPost/{postId}")]
        public async Task<IActionResult> GetAllPost(int postId)
        {
            if(postId == null) return NotFound();

            var post = await appDBContext.Posts
                .Include(i=> i.PostImages)
                .Include(l=> l.LikePosts)
                .Include(c=>c.CommandPosts)
                .Include(u=> u.User)
                .FirstOrDefaultAsync(i => i.Id == postId)
                ;
            if (post == null) return NotFound();

            var FullPost = new GetFullPostDto
            {
                Id = post.Id,
                Title = post.Title,
                Images = post.PostImages.Select(i => new GetImagePostDto
                {
                    Id = i.Id,
                    Url = i.Link_Image,
                }).ToList(),

                LikePosts = post.LikePosts.Select(l => new GetLikePostDto
                {
                    Id = l.Id
                }).ToList(),

                CommentPosts = post.CommandPosts.Select(c => new GetCommentPost
                {
                    Id = c.Id,
                }).ToList(),

                User = new GetUserPostDto
                {
                    username = post.User.UserName,
                    fullName = post.User.First_Name +" " +post.User.Last_Name,
                    imageProfile = post.User.Link_Image_Profile
                }

            };
            return Ok(FullPost);


        }
        [Authorize]
        [HttpPost("CreateLikeToPost/{PostId}")]
        public async Task<IActionResult> CreateLikeToPost(int PostId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(UserId)) return Unauthorized();
            var existingLike = await appDBContext.LikePost
                .FirstOrDefaultAsync(l => l.PostId == PostId && l.UserId == UserId);
            if (existingLike != null)
            {
                return BadRequest("You have already liked this post.");
            }
            var newLike = new LikePost
            {
                PostId = PostId,
                UserId = UserId
            };
            await appDBContext.LikePost.AddAsync(newLike);
            await appDBContext.SaveChangesAsync();
            return Ok("Like added successfully.");
        }
        [Authorize]
        [HttpPost("DeleteLikeToPost/{PostId}")]
        public async Task<IActionResult> DeleteLikeToPost(int PostId)
        {
            var UserId =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(UserId)) { return Unauthorized(); }

            var isLiKe = await appDBContext.LikePost.FirstOrDefaultAsync(x=> x.PostId == PostId && UserId == x.UserId);
            if (isLiKe == null) return NotFound();

             appDBContext.LikePost.Remove(isLiKe);
            await appDBContext.SaveChangesAsync();

            return Ok();

        }
        [Authorize]
        [HttpPost("CreateLikeToCommend")]
        public async Task <IActionResult> CreateLikeToComment(int PostId , CreateLikeCommentDto createLikeCommentDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var isLiked = await appDBContext.LikeCommand.FirstOrDefaultAsync(x => x.CommandId == createLikeCommentDto.CommandId && x.UserId == userId);
            if(isLiked != null) return BadRequest("You have already liked this comment.");

            var newLike = new LikeCommand
            {
                CommandId = createLikeCommentDto.CommandId,
                UserId = userId,
                PostId = PostId
            };
            await appDBContext.LikeCommand.AddAsync(newLike);
            await appDBContext.SaveChangesAsync();
            return Ok("Like added successfully.");
        }
        [Authorize]
        [HttpPost("DeleteLikeToCommend")]
        public async Task<IActionResult> DeleteLikeToComment(int PostId, CreateLikeCommentDto createLikeCommentDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var isLiked = await appDBContext.LikeCommand.FirstOrDefaultAsync(x => x.CommandId == createLikeCommentDto.CommandId && x.UserId == userId);
            if (isLiked == null) return NotFound("Like not found.");
            appDBContext.LikeCommand.Remove(isLiked);
            await appDBContext.SaveChangesAsync();
            return Ok("Like removed successfully.");
        }

        [Authorize]
        [HttpGet("CreateFollowing/{userName}")]
        public async Task<IActionResult> CreateFollowing(string userName)
        {
            if (userName == null) return BadRequest("username is empty");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest("user is not found");

            var userOther = await appDBContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            var follow = new Follow
            {
                FollowerId = userId,
                FollowingId = userOther.Id,
            };

            await appDBContext.Follows.AddAsync(follow);
            await appDBContext.SaveChangesAsync();
            return Ok();

        }

        [Authorize]
        [HttpDelete("DeleteFollow/{userName}")]
        public async Task<IActionResult> DelelteFollowing(string userName)
        {
            if (userName == null) return BadRequest("username is empty");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return BadRequest("user is not found");

            var userOther = await appDBContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            var follow = await appDBContext.Follows.FirstOrDefaultAsync(u => u.FollowerId == userId && u.FollowingId == userOther.Id);

            if (follow == null) return BadRequest("Not found");
             appDBContext.Follows.Remove(follow);
            await appDBContext.SaveChangesAsync();
            return Ok();

        }


    }
}
