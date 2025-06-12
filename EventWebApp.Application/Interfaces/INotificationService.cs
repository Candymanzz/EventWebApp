using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApp.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUsersAsync(IEnumerable<string> userEmails, string subject, string message);
    }
}
