using Autofac;
using NesService.Infrastructure.Extensions;
using System.ServiceProcess;

namespace NesService
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var debug = false;
            var container = ServiceExtension.ConfigureContainer();

            if (debug)
            {
                var service = container.Resolve<Service>();
                service.OnDebug();
            }
            else
            {
                var service = container.Resolve<Service>();
                ServiceBase.Run(service);
            }
        }

    }
}
