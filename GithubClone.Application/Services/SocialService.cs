using GithubClone.Application.DTOs;
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

        private readonly INotificationService _notificationService;
        public SocialService(ISocialRepository repo, INotificationService notificationService)
        {
            _repo = repo;
            _notificationService = notificationService; 
            
        }
        public async Task StarRepo(int userId, int repoId)
        {
            await _repo.AddStar(userId, repoId);

            //Notify the repo owner 
            var repo = await _repo.GetRepoById(repoId);
            if(repo!=null && repo.OwnerId != userId)
            {
                await _notificationService.SendNotificationAsync(
                    repo.OwnerId.ToString(),
                    $"User{userId} starred your repository");
            }

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

            if(userId != followingId)
            {
                await _notificationService.SendNotificationAsync(followingId.ToString(),
                    $"User {userId} started following you");
            }
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

        public async Task<UserFollowStatsDto> GetFollowStatsAsync(int userId)
        {
            return await _repo.GetUserFollowStatsAsync(userId);
     
        }
    }
}
