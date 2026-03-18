using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
