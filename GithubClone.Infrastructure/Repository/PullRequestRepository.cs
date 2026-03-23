using Dapper;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Domain.Entities;
using GithubClone.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Infrastructure.Repository
{
    public class PullRequestRepository : IPullRequestRepository
    {
        private readonly DapperContext _context;
        public PullRequestRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(PullRequest pr)
        {
            var sql = @"INSERT INTO PullRequests 
                       (RepositoryId, SourceBranchId, TargetBranchId, Title, Description,Status) 
                        VALUES (@RepositoryId,@SourceBranchId,@TargetBranchId,@Title,@Description,'Open');
                                    SELECT CAST(SCOPE_IDENTITY() as int)";

            using var conneciton = _context.CreateConnection();

            return await conneciton.ExecuteScalarAsync<int>(sql, pr);
        }

        public async Task<IEnumerable<PullRequest>> GetByRepositoryIdAsync(int repoId)
        {
            var query = "SELECT * FROM PullRequests WHERE RepositoryId = @repoId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PullRequest>(query, new { repoId });
        }


        public async Task<PullRequest> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM PullRequests WHERE Id = @id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<PullRequest>(query, new { id });
        }


        public async Task AddCommentAsync(PullRequestComment comment)
        {
            var query = @"INSERT INTO PullRequestComments(PullRequestId,UserId,Comment) 
                        VALUES (@PullRequestId,@UserId,@Comment)";
            
            using var connection = _context.CreateConnection(); 

            await connection.ExecuteAsync (query,  comment);

        }



        public async Task<IEnumerable<Commit>> GetCommitByBranchId(int branchId)
        {
            var sql = "SELECT * FROM Commits WHERE BranchId = @branchId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Commit>(sql, branchId);
        }




        public async Task CopyCommits(int sourceBranchId, int targetBranchId)
        {
            

            var sql = @"INSERT INTO Commits (Message, RepositoryId, BranchId, CreatedAt)
                    SELECT Message, RepositoryId, @targetBranchId, GETUTCDATE()
                    FROM Commits WHERE BranchId = @sourceBranchId";
            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(sql, new { sourceBranchId, targetBranchId });
        }
 
      

        public async Task UpdateStatus(int prId, string status)
        {
            var sql = "UPDATE PullRequests SET Status = @status WHERE Id = @prId";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new { prId, status });
        }

        //Handle the transaction 
        public async Task MergePullRequest(int prId, int sourceBranchId, int targetBranchId)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                //  Copy commits from source → target
                var copyCommitsSql = @"
                INSERT INTO Commits (Message, RepositoryId, BranchId, CreatedAt,CreatedBy)
                SELECT Message, RepositoryId, @targetBranchId, GETUTCDATE(),CreatedBy
                FROM Commits
                WHERE BranchId = @sourceBranchId";

                await connection.ExecuteAsync(copyCommitsSql, new
                {
                    sourceBranchId,
                    targetBranchId
                }, transaction);

                //  Update PR status
                var updatePrSql = @"
                UPDATE PullRequests
                SET Status = 'Merged'
                WHERE Id = @prId";

                await connection.ExecuteAsync(updatePrSql, new { prId }, transaction);

                // EVERYTHING SUCCESS → SAVE
                transaction.Commit();
            }
            catch
            {
                //  ANY ERROR → ROLLBACK
                transaction.Rollback();
                throw;
            }
        }
    }








}
