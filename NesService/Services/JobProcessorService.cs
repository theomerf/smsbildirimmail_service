using Autofac;
using Entities.Models;
using Hangfire;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class JobProcessorService
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILoggerService _logger;
        private readonly int _maxRetryCount;
        private readonly int _retryDelayMs;
        public JobProcessorService(ILifetimeScope lifetimeScope, ILoggerService logger) 
        {
            _logger = logger;
            _lifetimeScope = lifetimeScope;

            string maxRetryValue = ConfigurationManager.AppSettings["MaxRetryCount"];
            if (!int.TryParse(maxRetryValue, out _maxRetryCount) || _maxRetryCount <= 0)
            {
                _maxRetryCount = 5;
                _logger.LogWarning($"Max. Retry değeri geçersiz: '{maxRetryValue}'. Varsayılan {_maxRetryCount} kullanılıyor.");
            }

            string retryDelayValue = ConfigurationManager.AppSettings["RetryDelay"];
            if (!int.TryParse(retryDelayValue, out _retryDelayMs) || _retryDelayMs <= 0)
            {
                _retryDelayMs = 1000;
                _logger.LogWarning($"Retry gecikme değeri geçersiz: '{retryDelayValue}'. Varsayılan {_retryDelayMs}ms kullanılıyor.");
            }

            _logger.LogDebug($"PollingWatcherService konfigüre edildi. Maks deneme: {_maxRetryCount}, Denemeler arasındaki süre: {_retryDelayMs}ms");
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ProcessRequestWithManualRetry(Guid requestId, RequestType requestType, int attemptCount)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var repositoryManager = scope.Resolve<IRepositoryManager>();
                var serviceManager = scope.Resolve<IServiceManager>();
                try
                {
                    _logger.LogInfo($"Request işleniyor - ID: {requestId}, Type: {requestType}, Deneme: {attemptCount}/{_maxRetryCount}");

                    var requestDictionary = await repositoryManager.Request.GetRequestByIdAsync(requestId);
                    if (requestDictionary == null)
                    {
                        _logger.LogWarning($"Request bulunamadı - ID: {requestId}");
                        return;
                    }
                    var request = requestDictionary.First();

                    if (request.Value != Status.Pending && request.Value != Status.Processing)
                    {
                        _logger.LogInfo($"{requestType} requesti zaten işlenmiş - ID: {requestId}, Status: {request.Value}");
                        return;
                    }

                    await repositoryManager.Request.UpdateStatusAsync(requestId, Status.Processing);
                    await repositoryManager.SaveAsync();

                    switch (requestType)
                    {
                        case RequestType.Mail:
                            await serviceManager.MailService.SendMailAsync(requestId);
                            break;
                        case RequestType.Sms:
                            await serviceManager.SmsService.SendSmsAsync(requestId);
                            break;
                        case RequestType.Notification:
                            await serviceManager.NotificationService.SendNotificationAsync(requestId);
                            break;
                        default:
                            throw new NotSupportedException($"Desteklenmeyen request tipi: {requestType}");
                    }

                    await repositoryManager.Request.UpdateStatusAsync(requestId, Status.Done);
                    await repositoryManager.SaveAsync();

                    string successMessage = attemptCount > 1
                        ? $"{requestType} requesti başarıyla işlendi - ID: {requestId}, Toplam deneme: {attemptCount}"
                        : $"{requestType} requesti başarıyla işlendi - ID: {requestId}";

                    _logger.LogInfo(successMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{requestType} requesti işlenirken hata - ID: {requestId}, Deneme: {attemptCount}/{_maxRetryCount}, Hata: {ex.Message}");

                    if (attemptCount < _maxRetryCount)
                    {
                        var delay = TimeSpan.FromMilliseconds(_retryDelayMs);
                        var nextAttempt = attemptCount + 1;

                        BackgroundJob.Schedule<JobProcessorService>(svc =>
                            svc.ProcessRequestWithManualRetry(requestId, requestType, nextAttempt),
                            delay
                        );

                        _logger.LogDebug($"{requestType} requesti yeniden programlandı - ID: {requestId}, Sonraki deneme: {nextAttempt}, Gecikme: {_retryDelayMs}ms");
                    }
                    else
                    {
                        try
                        {
                            await repositoryManager.Request.UpdateStatusAsync(requestId, Status.Error);
                            await repositoryManager.SaveAsync();
                        }
                        catch (Exception statusEx)
                        {
                            _logger.LogError($"{requestType} requestinin status bilgisi güncellenemedi - ID: {requestId}, Hata: {statusEx.Message}");
                        }

                        _logger.LogError($"{requestType} requesti {_maxRetryCount} kere denendi ancak başarısız - ID: {requestId}, Son hata: {ex.Message}");
                    }
                }
            }
        }

        public async Task ScanForPendingRequests()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var repositoryManager = scope.Resolve<IRepositoryManager>();

                var pendingRequests = await repositoryManager.Request.GetPendingRequests();
                var requestList = pendingRequests?.ToList();

                if (requestList != null && requestList.Count > 0)
                {
                    _logger.LogInfo($"{requestList.Count} adet bekleyen request bulundu.");

                    foreach (var request in requestList)
                    {
                        BackgroundJob.Enqueue<JobProcessorService>(processor =>
                            processor.ProcessRequestWithManualRetry(request.Key, request.Value, 1));
                    }
                }
                else
                {
                    _logger.LogDebug("Bekleyen request bulunamadı.");
                }
            }
        }
    }
}
