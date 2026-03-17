using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{

    public class RepositoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreatedAt { get; set; }
    }


    public class CreateRepositoryDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }
    }

    public class UpdateRepositoryDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }
    }

}
