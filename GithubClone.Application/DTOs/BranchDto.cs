using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{
    public class BranchDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBranchDto
    {
        public int RepositoryId { get; set; }
        public string Name { get; set; }    
    }




}
