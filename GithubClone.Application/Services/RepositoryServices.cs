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
    public class RepositoryServices : IRepositoryService
    {
        private readonly IRepositoryRepository _repository;
        private readonly IMapper _mapper;

        public RepositoryServices(IRepositoryRepository repository , IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
            
        }





        public async Task<RepositoryDto> CreateAsync(int userId, CreateRepositoryDto dto)
        {
            var repo = _mapper.Map<Repositories>(dto);
            repo.OwnerId = userId;
            repo.CreatedAt = DateTime.UtcNow;
            var id = await _repository.CreateAsync(repo);   
            repo.Id = id;   
            return _mapper.Map<RepositoryDto>(repo); 
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




    }
}
