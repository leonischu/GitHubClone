using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{
    public class RecommendationDto
    {
        public int RepoId { get; set; }
        public float Score { get; set; }
    }
}
