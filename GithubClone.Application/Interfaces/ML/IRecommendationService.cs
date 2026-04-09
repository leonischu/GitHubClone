using GithubClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.ML
{
    public interface IRecommendationService
    {
        Task<List<RecommendationDto>> GetRecommendations(int userId);
    }
}
