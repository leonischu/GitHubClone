using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface ISocialRepository
    {
        Task AddStar(int userId, int repoId);
        Task RemoveStar(int userId, int repoId);
        Task<int> GetStarCount(int repoId);


        Task FollowUser(int followerId, int followingId);
        Task UnfollowUser(int followerId, int followingId);

        Task<IEnumerable<int>> GetFollowingIds(int userId);

        Task<IEnumerable<Repositories>>GetRepositoriesByUserIds(IEnumerable<int> userIds);
    }
}
