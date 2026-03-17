using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface ICommitRepository
    {
        Task<int> CreateAsync(Commit commit);

        Task<IEnumerable<Commit>> GetByRepositoryIdAsync(int repoId);
    }
}
