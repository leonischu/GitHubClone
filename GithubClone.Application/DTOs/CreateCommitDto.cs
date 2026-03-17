using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{
    public class CreateCommitDto
    {
        public int RepositoryId { get; set; }

        public string Message { get; set; }

        public List<FileDto> Files { get; set; }
    }
}
