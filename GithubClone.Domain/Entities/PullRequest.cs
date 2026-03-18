using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class PullRequest
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public int SourceBranchId { get; set; }
        public int TargetBranchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }             // Open, Merged, Closed
        public DateTime CreatedAt { get; set; }
    }


    public class PullRequestComment
    {
        public int Id { get; set; }
        public int PullRequestId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }




}
