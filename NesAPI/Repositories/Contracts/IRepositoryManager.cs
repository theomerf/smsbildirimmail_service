using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IMailRepository Mail { get; }
        ISmsRepository  Sms { get; }
        INotificationRepository Notification { get; }
        IStatRepository Stat { get; }

        void Save();
        Task SaveAsync();
    }
}
