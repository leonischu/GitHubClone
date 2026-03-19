using GithubClone.Application.DTOs;
using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public  interface IIssueService
    {

        Task<int> CreateIssue(CreateIssueDto dto, int userId);
        Task<IEnumerable<IssueDto>> GetIssues(int repositoryId);
        Task AddComment(CreateIssueCommentDto dto, int userId);
        Task CloseIssue(int issueId);
        Task<IEnumerable<IssueComment>> GetComments(int issueId);

    }
}
