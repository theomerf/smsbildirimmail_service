using API.Infrastructure.Extensions;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using NLog;
using System.IO;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogManager.Setup().LoadConfigurationFromFile(Server.MapPath("~/nlog.config"));
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RepositoryContext, Repositories.Migrations.Configuration>());
            ServiceExtension.ConfigureContainer();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}

