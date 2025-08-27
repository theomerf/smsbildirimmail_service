using System;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid id);
    }
}
