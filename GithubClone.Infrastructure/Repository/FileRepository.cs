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
    public class FileRepository : IFileRepository
    {
        private readonly DapperContext _context;
        public FileRepository(DapperContext context)
        {
            _context = context; 
        }

        public async Task<int> CreateAsync(FileEntity file)
        {
            var query = @"INSERT INTO Files(FileName,Content)
                        VALUES (@FileName,@Content);    
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, file);
        }

        public async Task<IEnumerable<FileEntity>> GetFilesByCommitIdAsync(int commitId)
        {
            var query = @"SELECT f.Id,f.FileName,f.Content FROM Files f 
                        INNER JOIN RepositoryFiles rf  ON rf.FileId = f.Id  
                        WHERE rf.CommitId = @CommitId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<FileEntity>(query, new { CommitId = commitId });
        }
    }
}
