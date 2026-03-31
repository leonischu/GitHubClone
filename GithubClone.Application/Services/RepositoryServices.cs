using AutoMapper;
using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GithubClone.Application.Services
{
    public class RepositoryServices : IRepositoryService
    {
        private readonly IRepositoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBranchRepository _branchRepo;
        private readonly ILogger<RepositoryServices> _logger;
        private readonly ICachingServices _cache;

        public RepositoryServices(
                IRepositoryRepository repository,
                IMapper mapper,
                IBranchRepository branchRepo,
                ILogger<RepositoryServices> logger,
                ICachingServices cache
            )
        {
            _repository = repository;
            _branchRepo = branchRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
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

                // Create Default main branch
                await _branchRepo.CreateAsync(new Branch
                {
                    RepositoryId = repo.Id,
                    Name = "main",
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                });

                _logger.LogInformation("Default branch 'main' created for RepoId: {RepoId}", repo.Id);

                // Clear the caches so new repo shows in list 
                await _cache.RemoveAsync($"repos_{userId}_1_10");

                return _mapper.Map<RepositoryDto>(repo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating repository for UserId:{UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<RepositoryDto>> GetUserRepository(int userId)
        {
            _logger.LogInformation("Fetching repositories for UserId: {UserId}", userId);

            try
            {
                var repos = await _repository.GetByUserIdAsync(userId);

                _logger.LogInformation("Fetched {Count} repositories for UserId:{UserId}", repos.Count(), userId);

                return _mapper.Map<IEnumerable<RepositoryDto>>(repos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching repositories for UserId:{UserId}", userId);
                throw;
            }
        }

        public async Task UpdateAsync(int repoId, UpdateRepositoryDto dto)
        {
            _logger.LogInformation("Updating repository Id:{repoId}", repoId);

            try
            {
                var repo = await _repository.GetByIdAsync(repoId);

                if (repo == null)
                {
                    _logger.LogWarning("Repository not found with Id: {RepoId}", repoId);
                    throw new Exception("Repository not found");
                }

                repo.Name = dto.Name;
                repo.Description = dto.Description;
                repo.IsPrivate = dto.IsPrivate;

                await _repository.UpdateAsync(repo);

                //  CLEAR CACHE AFTER UPDATE
                await _cache.RemoveAsync($"repos_{repo.OwnerId}_1_10");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating repository Id: {RepoId}", repoId);
                throw;
            }
        }

        public async Task DeleteAsync(int repoId)
        {
            _logger.LogInformation("Deleting repository Id: {RepoId}", repoId);

            try
            {
                var repo = await _repository.GetByIdAsync(repoId);

                if (repo == null)
                {
                    _logger.LogWarning("Repository not found with Id: {RepoId}", repoId);
                    throw new Exception("Repository not found");
                }



                await _repository.DeleteAsync(repoId);

                //  CLEAR CACHE AFTER DELETE
                await _cache.RemoveAsync($"repos_{repo.OwnerId}_1_10");
                // repos -> we are storing repositories , ownerId and 1 -> page number , 10 -> page size 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting repository Id:{RepoId}", repoId);
                throw;
            }
        }

        //MAIN REDIS IMPLEMENTATION
        public async Task<IEnumerable<RepositoryDto>> GetRepositories(int userId, int pageNumber, int pageSize)
        {
            var cacheKey = $"repos_{userId}_{pageNumber}_{pageSize}";

            _logger.LogInformation("Checking cache for key: {CacheKey}", cacheKey);

            try
            {
                //  1. CHECK CACHE i.e CACHE HIT ;Returns directly 
                var cachedData = await _cache.GetAsync<IEnumerable<RepositoryDto>>(cacheKey);

                if (cachedData != null)
                {
                    _logger.LogInformation("Cache HIT for key: {CacheKey}", cacheKey);
                    return cachedData;
                }

                _logger.LogInformation("Cache MISS for key: {CacheKey}", cacheKey);

                //  FETCH FROM DB IF CACHE IS MISSED 
                var repos = await _repository.GetRepositories(userId, pageNumber, pageSize);


                // Entity -> Dto

                var mapped = _mapper.Map<IEnumerable<RepositoryDto>>(repos);

                //  STORE IN CACHE i.e Store DTO in Redis 
                await _cache.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(5));

                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching paginated repos for UserId: {UserId}", userId);
                throw;
            }
        }
    }
}