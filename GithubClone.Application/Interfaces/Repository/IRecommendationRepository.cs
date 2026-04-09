using GithubClone.Infrastructure.ML_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Repository
{
    public interface IRecommendationRepository
    {
        Task<List<RepoInteraction>> GetUserInteractions();
        Task<List<int>> GetAllRepoIds();
    }
}
