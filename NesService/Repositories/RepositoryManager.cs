using Repositories.Contracts;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IMailRepository _mailRepository;
        private readonly ISmsRepository _smsRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IRequestRepository _requestRepository;

        public RepositoryManager(RepositoryContext context, IMailRepository mailRepository, ISmsRepository smsRepository, INotificationRepository notificationRepository, IRequestRepository requestRepository)
        {
            _context = context;
            _mailRepository = mailRepository;
            _smsRepository = smsRepository;
            _notificationRepository = notificationRepository;
            _requestRepository = requestRepository;
        }

        public IMailRepository Mail => _mailRepository;
        public ISmsRepository Sms => _smsRepository;
        public INotificationRepository Notification => _notificationRepository;
        public IRequestRepository Request => _requestRepository;

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
