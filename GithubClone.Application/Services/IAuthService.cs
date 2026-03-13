using GithubClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
  public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto dto);
        Task<string>LoginAsync(LoginDto dto);
    }
}
