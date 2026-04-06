using GithubClone.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SocialController : ControllerBase
    {
        private readonly ISocialService _service;
        public SocialController(ISocialService service)
        {
            _service = service;
        }

        [HttpPost("star/{repoId}")]

        public async Task<IActionResult> Star(int repoId)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            await _service.StarRepo(userId, repoId);
            return Ok("Starred");
        }

        [HttpPost("unstar/{repoId}")]
        public async Task<IActionResult> Unstar(int repoId)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            await _service.UnstarRepo(userId, repoId);
            return Ok("Unstarred");

        }

        [HttpGet("stars/{repoId}")]
        public async Task<IActionResult> GetStars(int repoId)
        {
            var count = await _service.GetStars(repoId);
            return Ok(count);
        }

        [HttpPost("follow/{userId}")]
        public async Task<IActionResult> Follow(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst("id").Value);

            await _service.Follow(currentUserId, userId);
            return Ok("Followed");
        }

        [HttpPost("unfollow/{userId}")]
        public async Task<IActionResult> Unfollow(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst("id").Value);

            await _service.UnFollow(currentUserId, userId);
            return Ok("Unfollowed");
        }

        [HttpGet("feed")]
        public async Task<IActionResult> Feed()
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            var feed = await _service.GetFeed(userId);
            return Ok(feed);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetFollowStats(int userId)
        {
            var result = await _service.GetFollowStatsAsync(userId);
            return Ok(result);
        }


    }
}
