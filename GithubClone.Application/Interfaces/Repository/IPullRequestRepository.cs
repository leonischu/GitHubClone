using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface IPullRequestRepository
    {
        Task<int> CreateAsync(PullRequest pr);
        Task<IEnumerable<PullRequest>> GetByRepositoryIdAsync(int repoId); //One Repo may have many pull request so it is listed 

        Task<PullRequest>GetByIdAsync(int id); // Get one sepecific PR 
        Task AddCommentAsync(PullRequestComment comment); 
        Task<IEnumerable<Commit>>GetCommitByBranchId(int branchId); //List all commit of branch 

        Task CopyCommits(int sourceBranchId, int targetBranchId); // take commit from the source and copy it to target 

        Task UpdateStatus(int prId, string status); // Change the PR status open,closed, merged
    }
}
