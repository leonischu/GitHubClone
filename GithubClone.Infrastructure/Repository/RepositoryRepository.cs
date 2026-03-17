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
    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly DapperContext _context;
        public RepositoryRepository(DapperContext context)
        {
            _context = context;

        }
        
        public async Task<int> CreateAsync(Repositories repo)
        {
            var query = @"INSERT INTO Repositories(Name,Description,OwnerId,IsPrivate,CreatedAt) 
                         VALUES 
                        (@Name,@Description,@OwnerId,@IsPrivate,@CreatedAt);
                            SELECT CAST(SCOPE_IDENTITY() as int);";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, repo);
        }



        public async Task<IEnumerable<Repositories>> GetByUserIdAsync(int userId)
        {
            var query = @"SELECT * FROM Repositories WHERE OwnerId = @userId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Repositories>(query, new { userId });
        }


        public async Task<Repositories> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Repositories WHERE Id = @id";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Repositories>(query, new { id });
        }

        public async Task UpdateAsync(Repositories repo)
        {
            var query = @"
                    UPDATE Repositories
                               SET Name = @Name,
                               Description = @Description,
                               IsPrivate = @IsPrivate
                              WHERE Id = @Id
                              ";

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(query, repo);
        }


        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM Repositories WHERE Id = @id";

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(query, new { id });
        }





    }
}
