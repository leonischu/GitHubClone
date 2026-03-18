using GithubClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public interface IBranchService
    {
        Task<BranchDto>CreateAsync(int userId,CreateBranchDto dto);
        Task<IEnumerable<BranchDto>> GetBranches(int repoId);

    }
}
