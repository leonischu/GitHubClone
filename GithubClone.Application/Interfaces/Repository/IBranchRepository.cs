using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface IBranchRepository
    {
        Task<int> CreateAsync(Branch branch);

        Task<IEnumerable<Branch>> GetByRepositoryIdAsync(int repoId);

        Task <Branch> GetByIdAsync(int id);
    }
}
