using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StatRepository : RepositoryBase<Stat>, IStatRepository
    {
        public StatRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Stat>> GetAllStatsAsync()
        {
            var stats = await _context.Requests
                .Include(r => r.MailRequest)
                .Include(r => r.NotificationRequest)
                .Include(r => r.SmsRequest)
                .GroupBy(r =>
                    r.Type == RequestType.Mail ? r.MailRequest.To :
                    r.Type == RequestType.Notification ? r.NotificationRequest.To : 
                    r.Type == RequestType.Sms ? r.SmsRequest.To : null)
                .Select(g => new Stat()
                {
                    To = g.Key,
                    SuccessCount = g.Count(r => r.Status == Status.Done),
                    FailureCount = g.Count(r => r.Status == Status.Error),
                    MailCount = g.Count(r => r.Type == RequestType.Mail),
                    NotificationCount = g.Count(r => r.Type == RequestType.Notification),
                    SmsCount = g.Count(r => r.Type == RequestType.Sms)
                })
                .ToListAsync();

            return stats;
        }
    }
}
