using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubClone.Domain.Entities;

namespace GithubClone.Application.Interfaces.Repository
{
    public  interface IRepositoryRepository
    {

        Task<IEnumerable<Repositories>> GetRepositories(int userId, int pageNumber, int pageSize);
        Task<int> CreateAsync(Repositories repo);

        Task<IEnumerable<Repositories>>GetByUserIdAsync( int userId);

        Task<Repositories> GetByIdAsync(int id);  // Yo chai repository ko id

        Task UpdateAsync(Repositories repo);

        Task DeleteAsync(int id);
    }
}
