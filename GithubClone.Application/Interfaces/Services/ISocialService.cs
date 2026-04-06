using GithubClone.Application.DTOs;
using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public interface ISocialService
    {
        Task StarRepo(int userId, int repoId);

        Task UnstarRepo(int userId, int repoId);
        Task<int> GetStars(int repoId);

        Task Follow(int userId, int followingId);
        Task UnFollow (int userId, int followingId);

        Task<IEnumerable<Repositories>> GetFeed(int userId);
        Task<UserFollowStatsDto> GetFollowStatsAsync(int userId);
    }
}
