using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;

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

            var response = new APIResponse();
            try
            {

               var userId = GetUserId();
                await _service.CreateCommitAsync(userId, dto);

                response.Status = true;
                response.StatusCode = HttpStatusCode.Created;
                response.Data = "Commit Created Sucessfully";
                return Ok(response);


            } 
            catch (Exception ex) 
            { 
                response.Status = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add(ex.Message);
                return BadRequest(response);
            
                }
        }

        [EnableRateLimiting("api-policy")]

        [HttpGet("{repoId}")]


        public async Task<IActionResult>GetCommits(int repoId)
        {
            var response = new APIResponse();
            try
            {
                var commits = await _service.GetCommits(repoId);

                response.Status = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Data = commits;


                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add(ex.Message);
                return BadRequest(response);

            }




        }




        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetByBranch(int branchId)
        {
            var response = new APIResponse();
            try
            {
                var commits = await _service.GetCommitsByBranch(branchId);
                response.Status = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Data = commits;


                return Ok(response);
            }
            catch (Exception ex) {

                response.Status = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add(ex.Message);
                return BadRequest(response);
            }
        }

    }
}
