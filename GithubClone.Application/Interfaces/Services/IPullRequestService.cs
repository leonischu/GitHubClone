using GithubClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public interface IPullRequestService
    {
        Task<int> CreatePR(CreatePullRequestDto dto);
        Task<IEnumerable<PullRequestDto>> GetPrs(int repoId);
        Task AddComment(CreateCommentDto dto,int userId);
        Task ClosePR(int prId);
        Task MergePR(int prId, int sourceBranchId, int targetBranchId);
    }
}
