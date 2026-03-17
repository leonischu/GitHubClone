using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class RepositoryFile
    {
        public int Id { get; set; }

        public int RepositoryId { get; set; }

        public int FileId { get; set; }

        public int CommitId { get; set; }
    }
}
