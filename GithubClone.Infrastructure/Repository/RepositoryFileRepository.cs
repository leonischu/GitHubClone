using Dapper;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Domain.Entities;
using GithubClone.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Infrastructure.Repository
{
    public class RepositoryFileRepository : IRepositoryFileRepository
    {
        private readonly DapperContext _context;
        public RepositoryFileRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(RepositoryFile repoFile)
        {
            var query = @"INSERT INTO RepositoryFiles(RepositoryId, FileId, CommitId)  
                        VALUES (@RepositoryId,@FileId,@CommitId)";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query,repoFile);
        }
    }
}
