using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   

    public class RepositoryController : ControllerBase
    {
        private readonly IRepositoryService _service;
        private readonly  ILogger<RepositoryController> _logger;
        public RepositoryController(IRepositoryService service,ILogger<RepositoryController> logger )
        {
            _service = service;
            _logger = logger;
        }


        //Get the userId from JWT

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("id");

            if (userIdClaim == null)
            {
                _logger.LogWarning("UserId claim missing in token");
                throw new UnauthorizedAccessException("Invalid token");
            }

            return int.Parse(userIdClaim.Value);
        }

        [EnableRateLimiting("api-policy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetRepositories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            _logger.LogInformation("Fetching paginated for UserId:{UserId}", userId);

            var repos = await _service.GetRepositories(userId, pageNumber, pageSize);

            return Ok(repos);
        }




        [HttpPost]
        //[EnableRateLimiting("repo-policy")]
        public async Task<IActionResult>Create(CreateRepositoryDto dto)
        {
 
                var userId = GetUserId(); 

            _logger.LogInformation("API Hit: CreateRepo for UserId: {UserId}", userId);


            var repo = await _service.CreateAsync(userId, dto);
                return Ok(repo);
       
        }

        [EnableRateLimiting("api-policy")]

        [HttpGet]

        public async Task<IActionResult> GetMyRepository()
        {
            var userId = GetUserId();
            _logger.LogInformation("Fetching user repositories for UserId: {UserId}", userId);

            var repos = await _service.GetUserRepository(userId);
            return Ok(repos);   
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRepositoryDto dto)
        {
            _logger.LogInformation("Updating repository Id: {RepoId}", id);

            await _service.UpdateAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting repository Id: {RepoId}", id);
            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}
