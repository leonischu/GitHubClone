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
    public class BranchRepository : IBranchRepository
    {
        private readonly DapperContext _context;

        public BranchRepository(DapperContext context)
        {
            _context = context;
            
        }

        public async Task<int> CreateAsync(Branch branch)
        {
            var query = @"INSERT INTO Branches (RepositoryId,Name,CreatedBy,CreatedAt) 
                        VALUES (@RepositoryId,@Name,@CreatedBy,@CreatedAt);         
                        SELECT CAST(SCOPE_IDENTITY() as int);";

             using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(query,branch);
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Branches WHERE Id = @id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Branch>(query, new { id });
        }

        public async Task<IEnumerable<Branch>> GetByRepositoryIdAsync(int repoId)
        {
            var query = "SELECT * FROM Branches WHERE RepostoryId = @repoId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Branch>(query, new { repoId });
        }
    }
}
