using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Domain.Entities
{
    public class APIResponse
    {
        public bool Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }

        public List<string> Errors { get; set; }
        public APIResponse()
        {
            Errors = new List<string>();
        }
    }
}
