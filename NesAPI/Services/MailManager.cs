using AutoMapper;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using Services.Dtos;
using System.Threading.Tasks;

namespace Services
{
    public class MailManager : IMailService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public MailManager(IRepositoryManager manager, IMapper mapper, ILoggerService logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateMailAsync(MailDto mailDto)
        {
            var mail = _mapper.Map<MailRequest>(mailDto);

            if (!mail.Validate(out var error)) 
            {
                _logger.LogWarning(error);
            }

            var request = new Request()
            {
                Type = RequestType.Mail,
            };

            mail.Request = request;
            mail.CreatedAt = request.CreatedAt;

            _manager.Mail.CreateMail(mail);
            await _manager.SaveAsync();

            string msg = $"{mail.Id}'li mail başarıyla oluşturuldu.";
            _logger.LogDebug(msg);
        }
    }
}
