using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommitController : ControllerBase
    {
        private readonly ICommitService _service;
        public CommitController(ICommitService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("id").Value);
        }

        [HttpPost]

        public async Task<IActionResult>Create(CreateCommitDto dto)
        {
            var userId = GetUserId();
            await _service.CreateCommitAsync(userId, dto);
            return Ok();
        }

        [HttpGet("{repoId}")]

        public async Task<IActionResult>GetCommits(int repoId)
        {
            var commits = await _service.GetCommits(repoId);
            return Ok(commits); 
        }






    }
}
