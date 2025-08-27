using AutoMapper;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SmsManager : ISmsService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public SmsManager(IRepositoryManager manager, IMapper mapper, ILoggerService logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateSmsAsync(SmsDto smsDto)
        {
            var sms = _mapper.Map<SmsRequest>(smsDto);

            if (!sms.Validate(out var error))
            {
                _logger.LogWarning(error);
            }

            var request = new Request()
            {
                Type = RequestType.Sms,
            };

            sms.Request = request;
            sms.CreatedAt = request.CreatedAt;

            _manager.Sms.CreateSms(sms);
            await _manager.SaveAsync();

            string msg = $"{sms.Id}'li sms başarıyla oluşturuldu.";
            _logger.LogDebug(msg);
        }
    }
}
