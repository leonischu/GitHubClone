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
    public class PullRequestService:IPullRequestService
    {

        private readonly IPullRequestRepository _repo;
        private readonly IMapper _mapper;

        public PullRequestService(IPullRequestRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<int>CreatePR(CreatePullRequestDto dto)
        {
            //Convert DTO to Entity
            var pr = _mapper.Map<PullRequest>(dto);


            //Set default values 
            pr.Status = "Open";
            pr.CreatedAt = DateTime.Now;
            return await _repo.CreateAsync(pr); 
        }






        public async Task<IEnumerable<PullRequestDto>> GetPrs(int repoId)
        {
            var prs = await _repo.GetByRepositoryIdAsync(repoId);
            return _mapper.Map<IEnumerable<PullRequestDto>>(prs);
        }


      

        public async Task AddComment(CreateCommentDto dto, int userId)
        {
            var comment = _mapper.Map<PullRequestComment>(dto);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.Now;

            await _repo.AddCommentAsync(comment);   
        }

        public async Task ClosePR(int prId)
        {
            var pr = await _repo.GetByIdAsync(prId);

            if (pr == null)
                throw new Exception("Pull Request not found");

            if (pr.Status != "Open")
                throw new Exception("Only open PR can be closed");
            await _repo.UpdateStatus(prId, "Closed");
        }
        public async Task MergePR(int prId)
        {
            var pr = await _repo.GetByIdAsync(prId);

            if (pr == null)
                throw new Exception("Pull Request not found");
            if (pr.Status != "Open")
                throw new Exception("PR already closed or merged");

            //Logic for merge
            // 1. Copy commit from source to target
              
            await _repo.CopyCommits(pr.SourceBranchId,pr.TargetBranchId);

            // 2. Update status 
            await _repo.UpdateStatus(prId, "Merged");

        }

     

        
    }
}
