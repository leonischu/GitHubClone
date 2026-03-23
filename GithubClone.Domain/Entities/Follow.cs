using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class Follow
    {
        public int Id { get; set; }
        public int FollowerId { get; set; } 

        public int FollowingId  { get; set; }

        public DateTime CreatedAt { get; set; } 
    }
}
