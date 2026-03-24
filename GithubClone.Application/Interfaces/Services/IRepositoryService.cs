using GithubClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public interface IRepositoryService
    {
        Task<IEnumerable<RepositoryDto>> GetRepositories(int userId, int pageNumber, int pageSize);
        Task<RepositoryDto> CreateAsync(int userId, CreateRepositoryDto dto);
         Task<IEnumerable<RepositoryDto>> GetUserRepository(int userId);

        Task UpdateAsync(int repoId, UpdateRepositoryDto dto); 
        Task DeleteAsync(int repoId);   

    }
}
