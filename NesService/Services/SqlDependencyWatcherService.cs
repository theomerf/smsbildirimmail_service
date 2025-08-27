using Autofac;
using Entities.Models;
using Hangfire;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class SqlDependencyWatcherService : IDataWatcherService
    {
        private readonly ILoggerService _logger;
        private SqlDependencyService _sqlDependencyWatcher;

        public SqlDependencyWatcherService(ILoggerService logger)
        {
            _logger = logger;
        }

        public void StartWatching()
        {
            try
            {
                _logger.LogInfo("SQL Dependency watcher başlatılıyor...");

                BackgroundJob.Enqueue<JobProcessorService>(svc => svc.ScanForPendingRequests());
                _logger.LogInfo("SQL Dependency kurulmadan önce veritabanındaki eski pending requestler için kontrol yapılıyor.");

                _sqlDependencyWatcher = new SqlDependencyService(_logger, ProcessNewRequestCallback);
                _sqlDependencyWatcher.StartWatching();

                _logger.LogInfo("SQL Dependency watcher başlatıldı!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Dependency watcher başlatma hatası: {ex.Message}");
                throw;
            }
        }

        public void StopWatching()
        {
            try
            {
                _logger.LogInfo("SQL Dependency watcher durduruluyor...");

                StopSqlDependencyWatcher();

                _logger.LogInfo("SQL Dependency watcher durduruldu!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Dependency watcher durdurma hatası: {ex.Message}");
            }
        }

        public void StopSqlDependencyWatcher()
        {
            try
            {
                _sqlDependencyWatcher?.StopWatching();
                _sqlDependencyWatcher = null;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"SQL Dependency durdurulurken hata oluştu. Hata: {ex}");
            }
        }

        private void ProcessNewRequestCallback(Guid requestId, RequestType requestType)
        {
            try
            {
                _logger.LogInfo($"SQL Dependency tetiklemesi - Yeni request: ID {requestId}, Type: {requestType}");

                BackgroundJob.Enqueue<JobProcessorService>(processor => processor.ProcessRequestWithManualRetry(requestId, requestType, 1));
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Dependency callback hatası - Request ID: {requestId}, Hata: {ex.Message}");
            }
        }
    }
}