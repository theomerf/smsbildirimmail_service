using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IServiceManager
    {
        IMailService MailService { get; }
        INotificationService NotificationService { get; }
        ISmsService SmsService { get; }
        IStatsService StatsService { get; }
    }
}
