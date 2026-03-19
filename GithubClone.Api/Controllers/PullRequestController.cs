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
    public class PullRequestController : ControllerBase
    {
        private readonly IPullRequestService _service;

        public PullRequestController(IPullRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>Create(CreatePullRequestDto dto)
        {
            var id = await _service.CreatePR(dto);
            return Ok(new {PullRequestId = id}); 
        }

        [HttpGet("{repoId}")]
        public async Task<IActionResult>Get(int repoId)
        {
            var prs =  await _service.GetPrs(repoId);
            return Ok(prs);
        }

        [HttpPost("comment")]

        public async Task<IActionResult>AddComment(CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            await _service.AddComment(dto,userId); 
            return Ok("Comment Added");
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult>Close(int id)
        {
            await _service.ClosePR(id);
            return Ok("Pull request closed");
        }

        [HttpPost("{id}/merge")]

        public async Task<IActionResult>Merge(int id)
        {
            await _service.MergePR(id);
            return Ok("Pull request merged sucessfully");
        }




    }
}
