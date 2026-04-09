using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Infrastructure.ML_Models
{
    public class RepoInteraction
    {
        [KeyType (count:1000)]
        public uint UserId { get; set; }

        [KeyType (count:1000)]
        public uint RepositoryId { get; set; }

        public float Label {  get; set; } // 1 or 0
    }
}
