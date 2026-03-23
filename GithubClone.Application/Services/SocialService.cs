using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
    public class SocialService : ISocialService
    {
        private readonly ISocialRepository _repo;
        public SocialService(ISocialRepository repo)
        {
            _repo = repo;
            
        }
        public async Task StarRepo(int userId, int repoId)
        {
            await _repo.AddStar(userId, repoId);
        }


        public async Task UnstarRepo(int userId, int repoId)
        {
            await  _repo.RemoveStar(userId,repoId);
        }

        public async Task<int> GetStars(int repoId)
        {
            return await _repo.GetStarCount(repoId);
        }

        public async Task Follow(int userId, int followingId)
        {
           await _repo.FollowUser(userId, followingId);
        }



        public async Task UnFollow(int userId, int followingId)
        {
            await _repo.UnfollowUser(userId, followingId);
        }

        public async Task<IEnumerable<Repositories>> GetFeed(int userId)
        {
            var followingIds = await _repo.GetFollowingIds(userId);
            return await _repo.GetRepositoriesByUserIds(followingIds);
        }


    }
}
