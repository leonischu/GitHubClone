using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class IssueComment
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
