using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.DTOs
{
    public class StarDto
    {
        public int RepositoryId     { get; set; }   

    }

    public class FollowDto
    {
        public int FollowingId { get; set; }    
    }


    public class UserFollowStatsDto
    {
        public int Followers { get; set; }
        public int Following { get; set; }
    }

}
