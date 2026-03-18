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
    public class BranchService : IBranchService
    {

        private readonly IBranchRepository _repo;
        private readonly IMapper _mapper;
        public BranchService(IBranchRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<BranchDto> CreateAsync(int userId, CreateBranchDto dto)
        {
            var branch = new Branch
            {
                RepositoryId = dto.RepositoryId,
                Name = dto.Name,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
            };

        var id = await _repo.CreateAsync(branch); 
            branch.Id = id;
            return _mapper.Map<BranchDto>(branch);  
        }



        public async Task<IEnumerable<BranchDto>> GetBranches(int repoId)
        {
            var branches = await _repo.GetByRepositoryIdAsync(repoId);  
            return _mapper.Map<IEnumerable<BranchDto>>(branches);
        }
    }
}
