using Dapper;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Infrastructure.Database;
using GithubClone.Infrastructure.ML_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Infrastructure.Repository
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly DapperContext _context;
        public RecommendationRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<List<int>> GetAllRepoIds()
        {
            using var connection = _context.CreateConnection();
            var query = "Select Id FROM Repositories";
            var repos = await connection.QueryAsync<int>(query);
            return repos.ToList();
        }

        public async Task<List<RepoInteraction>> GetUserInteractions()
        {
            using var connection = _context.CreateConnection();

            var query = @"SELECT CAST(UserId AS FLOAT) AS UserId,
                                 CAST(RepositoryId AS FLOAT) AS RepositoryId,
                                 1 AS Label FROM Stars";
            var stars =  await connection.QueryAsync<RepoInteraction>(query);
            return stars.ToList();
        }
    }
}
