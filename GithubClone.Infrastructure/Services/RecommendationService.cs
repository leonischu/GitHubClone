using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.ML;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Infrastructure.ML_Models;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace GithubClone.Infrastructure.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly MLContext _mlContext;
        private readonly IRecommendationRepository _repo;

        public RecommendationService(IRecommendationRepository repo, MLContext mlContext)
        {
            _mlContext = mlContext;
            _repo = repo;
        }

        public async Task<List<RecommendationDto>> GetRecommendations(int userId)
        {
            // 1. Load data
            var interactions = await _repo.GetUserInteractions();

            if (interactions == null || !interactions.Any())
                return new List<RecommendationDto>();

            var data = _mlContext.Data.LoadFromEnumerable(interactions);

            // 2.  Convert IDs to Keys
            var dataProcessPipeline =
                _mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "userIdEncoded",
                    inputColumnName: nameof(RepoInteraction.UserId))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "repoIdEncoded",
                    inputColumnName: nameof(RepoInteraction.RepositoryId)));

            // 3. Trainer config
            var options = new Microsoft.ML.Trainers.MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "repoIdEncoded",
                LabelColumnName = nameof(RepoInteraction.Label),

                //Training quality 
                //Iterations -> learning cycles
                //Rank -> complexity of mode
                
                NumberOfIterations = 50,
                ApproximationRank = 100,


                //Prevents overfitting
                Lambda = 0.025f,
                Alpha = 0.01f
            };

            var trainer = _mlContext.Recommendation()
                .Trainers
                .MatrixFactorization(options);

            var trainingPipeline = dataProcessPipeline.Append(trainer);  //combines dataprocessing and ml algorithm 

            // 4. Train model   learning happens here , finds patterns like user who starred repo A also like repo b 
            var model = trainingPipeline.Fit(data);

            // 5. Prediction engine
            var predictionEngine = _mlContext.Model
                .CreatePredictionEngine<RepoInteraction, RepoPrediction>(model);

            // 6. Get repos
            var allRepos = await _repo.GetAllRepoIds();

            var predictions = new List<RecommendationDto>();

            foreach (var repoId in allRepos)   //predicts score 
            {
                var prediction = predictionEngine.Predict(new RepoInteraction
                {
                    UserId = (uint)userId,
                    RepositoryId = (uint)repoId
                });

                float score = prediction.Score;  //here higher the score better recommendation 

                // 7. Clean or handle  invalid values  an score 
                if (float.IsNaN(score) || float.IsInfinity(score))
                    score = 0;



                predictions.Add(new RecommendationDto
                {
                    RepoId = repoId,
                    Score = score
                });
            }

            // 8. Return top 5
            return predictions
                .OrderByDescending(x => x.Score)
                .Take(5)
                .ToList();
        }
    }
}