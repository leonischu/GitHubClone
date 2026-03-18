using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;

        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("id").Value);
        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateBranchDto dto)
        {
            var userId = GetUserId();
            var branch = await _service.CreateAsync(userId,dto);
            return Ok(branch);
        }


        [HttpGet("{repoId}")]

        public async Task<IActionResult>GetBranches(int repoId)
        {
            var branches = await _service.GetBranches(repoId);
            return Ok(branches);    
        }
    }
}
