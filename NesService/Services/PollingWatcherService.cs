using Autofac;
using Entities.Models;
using Hangfire;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class PollingWatcherService : IDataWatcherService
    {
        private readonly ILoggerService _logger;
        private readonly int _pollingIntervalMs;
        private static int _isProcessing = 0;
        private static volatile bool _isRunning = false;

        private static string _scheduledJobId = null;
        private static readonly object _jobLock = new object();

        public PollingWatcherService(ILoggerService logger)
        {
            _logger = logger;

            string timeValue = ConfigurationManager.AppSettings["Time"];
            if (!int.TryParse(timeValue, out _pollingIntervalMs) || _pollingIntervalMs <= 0)
            {
                _pollingIntervalMs = 10000;
                _logger.LogWarning($"Time değeri geçersiz: '{timeValue}'. Varsayılan {_pollingIntervalMs}ms kullanılıyor.");
            }

            _logger.LogInfo($"PollingWatcherService konfigüre edildi. Kontrol süresi: {_pollingIntervalMs}ms");
        }

        public void StartWatching()
        {
            try
            {
                _logger.LogInfo("Hangfire polling job başlatılıyor...");

                lock (_jobLock) 
                {
                    _isRunning = true;
                    _scheduledJobId = null;
                }

                BackgroundJob.Enqueue<PollingWatcherService>(svc => svc.RunPollingCycle());

                _logger.LogInfo("Hangfire polling job başlatıldı!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hangfire job başlatma hatası: {ex.Message}");
                throw;
            }
        }

        public void StopWatching()
        {
            try
            {
                _logger.LogInfo("Polling watcher durduruluyor...");

                lock (_jobLock)
                {
                    _isRunning = false;

                    if (!string.IsNullOrEmpty(_scheduledJobId)) 
                    {
                        try
                        {
                            BackgroundJob.Delete(_scheduledJobId);
                            _logger.LogInfo($"Planlanmış iş iptal edildi: {_scheduledJobId}");
                            _scheduledJobId = null;
                        }
                        catch (Exception ex) 
                        {
                            _logger.LogError($"Planlanmış iş temizlenirken hata oluştu. Hata: {ex.Message}");
                        }
                    }
                }

                _logger.LogInfo("Polling watcher durduruldu!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Polling watcher durdurma hatası: {ex.Message}");
            }
        }

        public void RunPollingCycle()
        {
            if (Interlocked.Exchange(ref _isProcessing, 1) == 1)
            {
                _logger.LogDebug("Polling döngüsü zaten çalışıyor, atlanıyor.");
                return;
            }

            try
            {
                if (!_isRunning)
                {
                    _logger.LogDebug("Service durdurulmuş, polling atlanıyor.");
                    return;
                }

                _logger.LogDebug("Polling döngüsü başlatılıyor...");

                BackgroundJob.Enqueue<JobProcessorService>(processor => processor.ScanForPendingRequests());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Polling döngüsü hatası: {ex.Message}");
            }
            finally
            {
                Interlocked.Exchange(ref _isProcessing, 0);
                _logger.LogDebug("Polling döngüsü tamamlandı.");
            }

            lock (_jobLock) 
            {
                if (_isRunning)
                {
                    _scheduledJobId = BackgroundJob.Schedule<PollingWatcherService>(
                        svc => svc.RunPollingCycle(),
                        TimeSpan.FromMilliseconds(_pollingIntervalMs));

                    _logger.LogDebug($"Sonraki polling döngüsü planlandı. Id: {_scheduledJobId}");
                }
                else
                {
                    _logger.LogDebug($"Service durdurulmuş, yeni polling planlanmadı.");
                }
            }

        }


    }
}