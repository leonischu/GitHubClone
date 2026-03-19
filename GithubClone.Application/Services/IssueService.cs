using AutoMapper;
using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _repository;
        private readonly IMapper _mapper;

        public IssueService(IIssueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> CreateIssue(CreateIssueDto dto, int userId)
        {
            var issue = _mapper.Map<Issue>(dto);
            issue.CreatedAt = DateTime.UtcNow;

            var id = await _repository.CreateAsync(issue);
            return id;
        }

        public async Task<IEnumerable<IssueDto>> GetIssues(int repositoryId)
        {
            var issues = await _repository.GetByRepositoryIdAsync(repositoryId);
            return _mapper.Map<IEnumerable<IssueDto>>(issues);
        }

        public async Task AddComment(CreateIssueCommentDto dto, int userId)
        {
            var comment = _mapper.Map<IssueComment>(dto);
            comment.UserId = userId;

            await _repository.AddCommentAsync(comment);
        }

        public async Task CloseIssue(int issueId)
        {
            await _repository.UpdateStatusAsync(issueId, "Closed");
        }

        public async Task<IEnumerable<IssueComment>> GetComments(int issueId)
        {
            return await _repository.GetCommentsByIssueIdAsync(issueId);
        }
    }
}
