using AutoMapper;
using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
    public class RepositoryServices : IRepositoryService
    {
        private readonly IRepositoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBranchRepository _branchRepo;
        private readonly ILogger<RepositoryServices> _logger;

        public RepositoryServices(
                IRepositoryRepository repository ,    
                IMapper mapper, 
                IBranchRepository branchRepo,
                ILogger<RepositoryServices> logger
            )
        {
            _repository = repository;
            _branchRepo = branchRepo;
            _mapper = mapper;
            _logger = logger;
            
        }





        public async Task<RepositoryDto> CreateAsync(int userId, CreateRepositoryDto dto)
        {

            _logger.LogInformation("Creating repository for UserId:{UserId}", userId);

            try
            {

                var repo = _mapper.Map<Repositories>(dto);
                repo.OwnerId = userId;
                repo.CreatedAt = DateTime.UtcNow;
                var id = await _repository.CreateAsync(repo);
                repo.Id = id;
                _logger.LogInformation("Repository created with Id: {RepoId}", repo.Id);

                //Create Default main branch
                await _branchRepo.CreateAsync(new Branch
                {
                    RepositoryId = repo.Id,
                    Name = "main",
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                });

                _logger.LogInformation("Default branch 'main' created for RepoId: {RepoId}", repo.Id);


                return _mapper.Map<RepositoryDto>(repo);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while creating repository for UserId:{UserId}", userId);
                throw;
            }
            }


        public async Task<IEnumerable<RepositoryDto>> GetUserRepository(int userId)
        {
            var repos = await _repository.GetByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<RepositoryDto>>(repos);
        }

        public async Task UpdateAsync(int repoId, UpdateRepositoryDto dto)
        {
            var repo = await _repository.GetByIdAsync(repoId);

            if (repo == null)
                throw new Exception("Repository not found");
            repo.Name = dto.Name;   
            repo.Description = dto.Description; 
            repo.IsPrivate = dto.IsPrivate; 
            await _repository.UpdateAsync(repo);    
        }



        public async Task DeleteAsync(int repoId)
        {
            await _repository.DeleteAsync(repoId);

        }


        public async Task<IEnumerable<RepositoryDto>> GetRepositories(int userId, int pageNumber, int pageSize)
        {
            var repos = await _repository.GetRepositories(userId, pageNumber, pageSize);
            return _mapper.Map<IEnumerable<RepositoryDto>>(repos);
        }



    }
}
