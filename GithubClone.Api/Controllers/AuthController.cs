using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GithubClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;

        }

        [HttpPost("register")]

        public async Task<IActionResult>Register(RegisterDto dto)
        {
            var user = await _service.RegisterAsync(dto);
            return Ok(user);    

        }

        [HttpPost("login")]

        public async Task<IActionResult>Login(LoginDto dto)
        {
            var token = await _service.LoginAsync(dto); 
            return Ok(token);
        }


    }
}
