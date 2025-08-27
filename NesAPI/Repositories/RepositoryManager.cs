using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IMailRepository _mailRepository;
        private readonly ISmsRepository _smsRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IStatRepository _statRepository;

        public RepositoryManager(RepositoryContext context, IMailRepository mailRepository, ISmsRepository smsRepository, INotificationRepository notificationRepository, IStatRepository statRepository)
        {
            _context = context;
            _mailRepository = mailRepository;
            _smsRepository = smsRepository;
            _notificationRepository = notificationRepository;
            _statRepository = statRepository;
        }

        public IMailRepository Mail => _mailRepository;
        public ISmsRepository Sms => _smsRepository;
        public INotificationRepository Notification => _notificationRepository;
        public IStatRepository Stat => _statRepository;

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
