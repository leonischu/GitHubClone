using Dapper;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Domain.Entities;
using GithubClone.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Infrastructure.Repository
{
    public class SocialRepository : ISocialRepository
    {
        private readonly DapperContext _context;

        public SocialRepository(DapperContext context)
        {
            _context = context; 
        }
        //Get By Id 

        public async Task <Repositories?>GetRepoById(int repoId)
        {
            var sql = @"SELECT * FROM Repositories WHERE Id = @repoId";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Repositories>(
                sql,
                new { repoId }
            );
        }








        //To Add the stars



        public async Task AddStar(int userId, int repoId)
        {
            var sql = @"INSERT INTO Stars (Userid,RepositoryId) 
                        VALUES(@userId,@repoId)";
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new { userId, repoId });
        }

        //To remove the stars 

        public async Task RemoveStar(int userId, int repoId)
        {
            var sql = "DELETE FROM Stars WHERE UserId = @userId AND RepositoryId = @repoId";
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new {userId, repoId});
        }


        //For Star count 

        public async Task<int> GetStarCount(int repoId)
        {
            var sql = "SELECT COUNT(*) FROM Stars WHERE RepositoryId = @repoId";
            var connection = _context.CreateConnection();
           return await connection.ExecuteScalarAsync<int>(sql, new {repoId});
        }



        public async Task FollowUser(int followerId, int followingId)
        {
            using var connection = _context.CreateConnection();

            var sql = @"INSERT INTO Follows (FollowerId, FollowingId)
                    VALUES (@followerId, @followingId)";

            await connection.ExecuteAsync(sql, new { followerId, followingId });
        }

        public async Task<IEnumerable<int>> GetFollowingIds(int userId)
        {

            var sql = @"SELECT FollowingId FROM Follows WHERE FollowerId = @userId";

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<int>(sql, new { userId });
        }

        public async Task<IEnumerable<Repositories>> GetRepositoriesByUserIds(IEnumerable<int> userIds)
        {
            using var connection = _context.CreateConnection();

            var sql = @"SELECT * FROM Repositories WHERE OwnerId IN @userIds";

            return await connection.QueryAsync<Repositories>(sql, new { userIds });
        }



     

        public async Task UnfollowUser(int followerId, int followingId)
        {
            using var connection = _context.CreateConnection();

            var sql = @"DELETE FROM Follows 
                    WHERE FollowerId = @followerId AND FollowingId = @followingId";

             await connection.ExecuteAsync(sql, new { followerId, followingId });
        }
    }
}
