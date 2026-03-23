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
    public class IssueRepository : IIssueRepository
    {

        private readonly DapperContext _context;

        public IssueRepository(DapperContext context)
        {
            _context = context; 
        }

        public async Task<int> CreateAsync(Issue issue)
        {
            var query = @"INSERT INTO Issues (RepositoryId,Title,Description,Status,CreatedAt)
                        VALUES (@RepositoryId,@Title,@Description,@Status,@CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            using var connection = _context.CreateConnection();
            var id = await connection.QuerySingleAsync<int>(query,issue);
            return id;
        }

        public async Task<IEnumerable<Issue>> GetByRepositoryIdAsync(int repositoryId)
        {
            var query = "SELECT * FROM Issues WHERE RepositoryId = @repositoryId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Issue>(query, new { repositoryId });
        }

        public async Task <Issue> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Issues WHERE Id = @id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Issue>(query, new { id });
        }

        public async Task AddCommentAsync(IssueComment comment)
        {
            comment.CreatedAt = DateTime.Now;
            var query = @"INSERT INTO IssueComments (IssueId, UserId, Comment,CreatedAt)  
                       VALUES(@IssueId,@UserId,@Comment,@CreatedAt) ";
            using var connection = _context.CreateConnection();
            await connection.QueryAsync(query, comment);
        }
        public async Task<IEnumerable<IssueComment>> GetCommentsByIssueIdAsync(int issueId)
        {
            var query = "SELECT * FROM IssueComments WHERE IssueId = @issueId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<IssueComment>(query, new { issueId });
        }


        public async Task UpdateStatusAsync(int issueId, string status)
        {
            var query = "UPDATE Issues SET Status = @status WHERE Id = @issueId";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { status, issueId });
        }


    }
}
