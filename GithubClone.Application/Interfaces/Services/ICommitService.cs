using GithubClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public interface ICommitService
    {
        Task CreateCommitAsync(int userId, CreateCommitDto dto);

        Task<IEnumerable<CommitDto>> GetCommits(int repoId);
    
}
}
