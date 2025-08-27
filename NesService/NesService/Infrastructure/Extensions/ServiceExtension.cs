using Autofac;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;
using System;
using System.Configuration;
using System.Diagnostics;

namespace NesService.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            try
            {
                builder.RegisterType<RepositoryContext>().InstancePerLifetimeScope();
                builder.RegisterType<MailRepository>().As<IMailRepository>().InstancePerLifetimeScope();
                builder.RegisterType<SmsRepository>().As<ISmsRepository>().InstancePerLifetimeScope();
                builder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerLifetimeScope();
                builder.RegisterType<RequestRepository>().As<IRequestRepository>().InstancePerLifetimeScope();
                builder.RegisterType<RepositoryManager>().As<IRepositoryManager>().InstancePerLifetimeScope();

                builder.RegisterType<LoggerManager>().As<ILoggerService>().SingleInstance();
                builder.RegisterType<MailManager>().As<IMailService>().InstancePerLifetimeScope();
                builder.RegisterType<NotificationManager>().As<INotificationService>().InstancePerLifetimeScope();
                builder.RegisterType<SmsManager>().As<ISmsService>().InstancePerLifetimeScope();
                builder.RegisterType<ServiceManager>().As<IServiceManager>().InstancePerLifetimeScope();

                builder.RegisterType<JobProcessorService>().InstancePerLifetimeScope();

                string method = ConfigurationManager.AppSettings["CheckMethod"];
                if (string.Equals(method, "Polling", StringComparison.OrdinalIgnoreCase))
                {
                    builder.RegisterType<PollingWatcherService>()
                           .As<IDataWatcherService>()
                           .AsSelf()
                           .SingleInstance();
                }
                else
                {
                    builder.RegisterType<SqlDependencyWatcherService>()
                           .As<IDataWatcherService>()
                           .AsSelf()
                           .SingleInstance();
                }

                builder.Register(c => new Service(
                    c.Resolve<IDataWatcherService>(),
                    c.Resolve<ILoggerService>(),
                    c.Resolve<ILifetimeScope>()
                )).AsSelf();

                var container = builder.Build();
                return container;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NesService", $"Container yapılandırma hatası: {ex.Message}", EventLogEntryType.Error);
                throw;
            }
        }
    }
}