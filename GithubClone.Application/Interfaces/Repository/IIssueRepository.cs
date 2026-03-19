using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface IIssueRepository
    {
        Task<int> CreateAsync(Issue issue);
        Task<IEnumerable<Issue>> GetByRepositoryIdAsync(int repositoryId);
        Task<Issue> GetByIdAsync(int id);
        Task AddCommentAsync(IssueComment comment);
        Task<IEnumerable<IssueComment>> GetCommentsByIssueIdAsync(int issueId);
        Task UpdateStatusAsync(int issueId, string status);
    }
}
