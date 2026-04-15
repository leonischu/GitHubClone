using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _service;

        public IssueController(IIssueService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue(CreateIssueDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var id = await _service.CreateIssue(dto, userId);
            return Ok(new { Id = id });
        }

        [HttpGet("{repositoryId}")]
        public async Task<IActionResult> GetIssues(int repositoryId)
        {
            var issues = await _service.GetIssues(repositoryId);
            return Ok(issues);
        }

        [HttpPost("comment")]
        public async Task<IActionResult> AddComment(CreateIssueCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            await _service.AddComment(dto, userId);
            return Ok("Comment added");
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseIssue(int id)
        {
            await _service.CloseIssue(id);
            return Ok("Issue closed");
        }

        [HttpGet("comments/{issueId}")]
        public async Task<IActionResult> GetComments(int issueId)
        {
            var comments = await _service.GetComments(issueId);
            return Ok(comments);
        }


    }
}
