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
    public class CommitService : ICommitService
    {
        private readonly ICommitRepository _commitRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IRepositoryFileRepository _repoFileRepo;
        private readonly IMapper _mapper;

        public CommitService(
        ICommitRepository commitRepo,
        IFileRepository fileRepo,
        IRepositoryFileRepository repoFileRepo,
        IMapper mapper)
        {
            _commitRepo = commitRepo;
            _fileRepo = fileRepo;
             _repoFileRepo = repoFileRepo;
            _mapper = mapper;
        }


        public async Task CreateCommitAsync(int userId, CreateCommitDto dto)
        {
            var commit = new Commit
            {
                RepositoryId = dto.RepositoryId,
                Message = dto.Message,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
            };

            var commitId = await _commitRepo.CreateAsync(commit);
            foreach (var fileDto in dto.Files)
            {
                var file = new FileEntity
                {
                    FileName = fileDto.FileName,
                    Content = fileDto.Content
                };
                var fileId = await _fileRepo.CreateAsync(file);
                var repoFile = new RepositoryFile
                {
                    RepositoryId = dto.RepositoryId,
                    FileId = fileId,
                    CommitId = commitId
                };
                await _repoFileRepo.CreateAsync(repoFile);

            }
        }

        public async Task<IEnumerable<CommitDto>> GetCommits(int repoId)
        {
            var commits = await _commitRepo.GetByRepositoryIdAsync(repoId);

            return _mapper.Map<IEnumerable<CommitDto>>(commits);
        }
    }
}
