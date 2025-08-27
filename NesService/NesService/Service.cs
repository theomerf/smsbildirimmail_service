using Autofac;
using Hangfire;
using Hangfire.SqlServer;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Configuration;
using System.ServiceProcess;

namespace NesService
{
    public partial class Service : ServiceBase
    {
        private IDataWatcherService _watcherService;
        private readonly ILoggerService _logger;
        private readonly ILifetimeScope _lifetimeScope;
        private BackgroundJobServer _backgroundJobServer;

        public Service(IDataWatcherService dataWatcherService, ILoggerService logger,
                      ILifetimeScope lifetimeScope)
        {
            this.ServiceName = "NesService";
            _watcherService = dataWatcherService;
            _logger = logger;
            _lifetimeScope = lifetimeScope;

            InitializeComponent();
            ConfigureHangfire();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger.LogInfo("NesService başlatılıyor...");

                _backgroundJobServer = new BackgroundJobServer();

                _watcherService.StartWatching();

                _logger.LogInfo("NesService başarıyla başlatıldı.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service başlatma hatası: {ex.Message}");

                _backgroundJobServer?.Dispose();
                _backgroundJobServer = null;
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                _logger.LogInfo("NesService durduruluyor...");

                _watcherService?.StopWatching();
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                    _backgroundJobServer = null;
                    _logger.LogInfo("Hangfire BackgroundJobServer durduruldu");
                }

                _logger.LogInfo("NesService başarıyla durduruldu.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service durdurma hatası: {ex.Message}");
            }
        }

        public void OnDebug()
        {
            _logger.LogInfo("Debug modu başlatılıyor...");
            OnStart(null);
            _logger.LogInfo("Debug modu başlatıldı.");
        }

        private void ConfigureHangfire()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["HangfireConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("DatabaseString connection string bulunamadı.");
                }

                GlobalConfiguration.Configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    })
                    .UseActivator(new AutofacJobActivator(_lifetimeScope));

                _logger.LogInfo("Hangfire yapılandırıldı.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hangfire yapılandırma hatası: {ex.Message}");
                throw;
            }
        }
    }
}