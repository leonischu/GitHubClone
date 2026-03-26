using Dapper;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Domain.Entities;
using GithubClone.Infrastructure.Database;
//using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GithubClone.Application.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(User user)
        {
            var query = @"INSERT INTO Users (Username,Email,PasswordHash,CreatedAt, IsEmailVerified, EmailVerificationToken, EmailVerificationTokenExpiry) 
                        VALUES (@Username,@Email,@PasswordHash,@CreatedAt, @IsEmailVerified, @EmailVerificationToken, @EmailVerificationTokenExpiry);
                        SELECT CAST(SCOPE_IDENTITY() as int );";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, user);
        }

        public async  Task<User?> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("Email", email);     
            return await connection.QueryFirstOrDefaultAsync<User>(query, parameters);
        }

        public async Task<User?> GetByVerificationTokenAsync(string token)
        {
            var query = "SELECT * FROM Users WHERE EmailVerificationToken = @Token";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Token = token });
        }
        public async Task UpdateAsync(User user)
        {
            var query = @"
        UPDATE Users
        SET 
            Username = @Username,
            Email = @Email,
            PasswordHash = @PasswordHash,
            IsEmailVerified = @IsEmailVerified,
            EmailVerificationToken = @EmailVerificationToken,
            EmailVerificationTokenExpiry = @EmailVerificationTokenExpiry
        WHERE Id = @Id
    ";
            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(query, user);
        }

    }
}
