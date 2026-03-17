using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class FileEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Content { get; set; }
    }
}
