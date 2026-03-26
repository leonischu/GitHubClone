using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(User user);
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByVerificationTokenAsync(string token);
        Task UpdateAsync(User user);

    }
}
