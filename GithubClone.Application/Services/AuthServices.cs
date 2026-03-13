using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
    public class AuthServices : IAuthService
    {
        public Task<string> LoginAsync(LoginDto dto)
        {
            
        }

        public Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
