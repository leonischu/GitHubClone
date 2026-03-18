using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{


    public class PullRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }

    public class CreatePullRequestDto
    {
        public int RepositoryId { get; set; }
        public int SourceBranchId { get; set; }
        public int TargetBranchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateCommentDto
    {
        public int PullRequestId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
    }

}
