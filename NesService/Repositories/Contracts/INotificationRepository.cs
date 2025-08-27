using Entities.Models;
using System;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface INotificationRepository : IRepositoryBase<NotificationRequest>
    {
        Task<NotificationRequest> GetNotificationById(Guid id);
    }
}
