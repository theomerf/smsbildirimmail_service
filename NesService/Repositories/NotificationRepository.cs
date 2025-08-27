using Entities.Models;
using Repositories.Contracts;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Repositories
{
    public class NotificationRepository : RepositoryBase<NotificationRequest>, INotificationRepository
    {
        public NotificationRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<NotificationRequest> GetNotificationById(Guid id) 
        {
            var notification = await FindByCondition(n => n.Id == id, false)
                .FirstOrDefaultAsync();

            return notification;
        }
    }
}
