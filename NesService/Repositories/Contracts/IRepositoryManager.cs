using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IMailRepository Mail { get; }
        ISmsRepository  Sms { get; }
        INotificationRepository Notification { get; }
        IRequestRepository Request { get; }

        void Save();
        Task SaveAsync();
    }
}
