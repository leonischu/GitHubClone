using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message);
    }
}
