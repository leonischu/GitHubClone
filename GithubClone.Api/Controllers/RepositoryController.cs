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
    public class RepositoryController : ControllerBase
    {
        private readonly IRepositoryService _service;
        public RepositoryController(IRepositoryService service)
        {
            _service = service;
        }


        //Get the userId from JWT

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("id").Value);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetRepositories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            var repos = await _service.GetRepositories(userId, pageNumber, pageSize);

            return Ok(repos);
        }




        [HttpPost]
        public async Task<IActionResult>Create(CreateRepositoryDto dto)
        {
            var userId = GetUserId();
            var repo = await _service.CreateAsync(userId, dto);
            return Ok(repo);    

        }

        [HttpGet]

        public async Task<IActionResult> GetMyRepository()
        {
            var userId = GetUserId();
            var repos = await _service.GetUserRepository(userId);
            return Ok(repos);   
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRepositoryDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}
