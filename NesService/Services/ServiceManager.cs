using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;
        private readonly ISmsService _smsService;

        public ServiceManager(IMailService mailService, INotificationService notificationService, ISmsService smsService)
        {
            _mailService = mailService;
            _notificationService = notificationService;
            _smsService = smsService;
        }

        public IMailService MailService => _mailService;
        public INotificationService NotificationService => _notificationService;
        public ISmsService SmsService => _smsService;
    }
}
