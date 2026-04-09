using GithubClone.Application.Interfaces.ML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _service;

        public RecommendationController(IRecommendationService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]

        public async Task<IActionResult>GetRecommendation(int userId)
        {
            var result = await _service.GetRecommendations(userId);
            return Ok(new
            {
                status = true,
                data =  result
            });
        }
    }
}
