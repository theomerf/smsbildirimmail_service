using API.Infrastructure.Handlers;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace API.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    var types = Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract)
                        .ToList();
                    foreach (var type in types)
                    {
                        cfg.AddProfile(Activator.CreateInstance(type) as Profile);
                    }
                });
                config.AssertConfigurationIsValid();
                return config;
            }).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper();
            }).As<IMapper>().InstancePerRequest();

            builder.RegisterType<RepositoryContext>().InstancePerRequest();
            builder.RegisterType<MailRepository>().As<IMailRepository>().InstancePerRequest();
            builder.RegisterType<SmsRepository>().As<ISmsRepository>().InstancePerRequest();
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerRequest();
            builder.RegisterType<StatRepository>().As<IStatRepository>().InstancePerRequest();
            builder.RegisterType<RepositoryManager>().As<IRepositoryManager>().InstancePerRequest();

            builder.RegisterType<ServiceManager>().As<IServiceManager>().InstancePerRequest();
            builder.RegisterType<MailManager>().As<IMailService>().InstancePerRequest();
            builder.RegisterType<NotificationManager>().As<INotificationService>().InstancePerRequest();
            builder.RegisterType<SmsManager>().As<ISmsService>().InstancePerRequest();
            builder.RegisterType<StatsManager>().As<IStatsService>().InstancePerRequest();
            builder.RegisterType<LoggerManager>().As<ILoggerService>().SingleInstance();


            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            AddGlobalExceptionHandler(container);
            AddGlobalExceptionFilter(container);
        }

        private static void AddGlobalExceptionHandler(IContainer container)
        {
            var loggerService = container.Resolve<ILoggerService>();
            var handler = new GlobalExceptionHandler(loggerService);

            GlobalConfiguration.Configuration.MessageHandlers.Add(handler);
        }

        private static void AddGlobalExceptionFilter(IContainer container)
        {
            var loggerService = container.Resolve<ILoggerService>();
            var filter = new GlobalExceptionFilterAttribute(loggerService);
            GlobalConfiguration.Configuration.Filters.Add(filter);
        }
    }
}