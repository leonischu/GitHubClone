using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface IFileRepository
    {
        Task<int> CreateAsync(FileEntity file);

        Task<IEnumerable<FileEntity>> GetFilesByCommitIdAsync(int commitId);
    }
}
