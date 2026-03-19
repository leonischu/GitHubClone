using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class Issue
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; }
    }
}
