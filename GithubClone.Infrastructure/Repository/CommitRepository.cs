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
    public class CommitRepository:ICommitRepository
    {
        private readonly DapperContext _context;

        public CommitRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Commit commit)
        {
            var query = @"INSERT INTO Commits(RepositoryId,Message,CreatedBy,CreatedAt)
                        VALUES(@RepositoryId,@Message,@CreatedBy,@CreatedAt)   
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, commit);
        }

        public async Task<IEnumerable<Commit>> GetByRepositoryIdAsync(int repoId)
        {
            var query = "SELECT * FROM Commits WHERE RepositoryId = @repoId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Commit>(query, new { repoId });
        }
    }
}
