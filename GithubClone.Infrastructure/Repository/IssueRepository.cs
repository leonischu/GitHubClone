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

        public async Task<IEnumerable<Issue>> GetIssuesWithCommentsByRepoId(int repositoryId)
        {
            var query = @"
                        SELECT 
                            i.Id, i.RepositoryId, i.Title, i.Description, i.Status, i.CreatedAt,
                            ic.Id, ic.IssueId, ic.UserId, ic.Comment, ic.CreatedAt
                        FROM Issues i
                        LEFT JOIN IssueComments ic ON i.Id = ic.IssueId
                        WHERE i.RepositoryId = @repositoryId";

            using var connection = _context.CreateConnection();

            var issueDictionary = new Dictionary<int, Issue>();  //IssueId = Issue Object 

            var result = await connection.QueryAsync<Issue, IssueComment, Issue>( // each row has issue and issue comment , combine it   QueryAsync<TFirst, TSecond, TReturn>
                query,
                (issue, comment) => //row 1  => issue + comment 1 , row2 = issue +  comment 2 and so on 
                {
                    if (!issueDictionary.TryGetValue(issue.Id, out var existingIssue))
                    {

                        //if issue dosent existm store new issue and give it empty comment list ,  if it exist skip creating new issue , use the existing one
                        existingIssue = issue;
                        existingIssue.Comments = new List<IssueComment>();
                        issueDictionary.Add(existingIssue.Id, existingIssue);
                    }

                    if (comment != null)
                    {

                        //If row has a comment add it to the correct issue 
                        existingIssue.Comments.Add(comment);
                    }

                    return existingIssue;
                },
                new { repositoryId },
                splitOn: "Id"  // from this column onward it a new object (Issue Comment) 
                               //i.e     Id | RepositoryId | Title | Id | IssueId | Comment  ? Where does issue and issue comment starts ??  
                               // when you see second id on table , start mapping next object (issue comment)
            );

            return issueDictionary.Values;
        }    }
}
