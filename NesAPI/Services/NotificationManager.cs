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
    public class NotificationManager : INotificationService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public NotificationManager(IRepositoryManager manager, IMapper mapper, ILoggerService logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = _mapper.Map<NotificationRequest>(notificationDto);

            if (!notification.Validate(out var error))
            {
                _logger.LogWarning(error);
            }

            var request = new Request()
            {
                Type = RequestType.Notification,
            };

            notification.Request = request;
            notification.CreatedAt = request.CreatedAt;

            _manager.Notification.CreateNotification(notification);
            await _manager.SaveAsync();

            string msg = $"{notification.Id}'li bildirim başarıyla oluşturuldu.";
            _logger.LogDebug(msg);
        }
    }
}
