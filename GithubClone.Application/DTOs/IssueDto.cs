using GithubClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{
    public class IssueDto
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<IssueComment> Comments { get; set; } = new();
    }

    public class CreateIssueDto
    {
        public int RepositoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateIssueCommentDto
    {
        public int IssueId { get; set; }
        public string Comment { get; set; }
    }

}
