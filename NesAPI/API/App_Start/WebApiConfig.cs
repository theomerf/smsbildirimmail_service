using API.Infrastructure.Extensions;
using API.Infrastructure.Handlers;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiThrottle;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            var throttlePolicy = new ThrottlePolicy(perSecond: 30, perMinute: 100, perHour: 200, perDay: 1000)
            {
                IpThrottling = true,
                ClientThrottling = false,
                EndpointThrottling = false,
                StackBlockedRequests = false
            };

            throttlePolicy.EndpointWhitelist.Add("*:/swagger*");

            throttlePolicy.EndpointWhitelist.Add("get:/content/*");
            throttlePolicy.EndpointWhitelist.Add("get:/scripts/*");
            throttlePolicy.EndpointWhitelist.Add("get:*.css");
            throttlePolicy.EndpointWhitelist.Add("get:*.js");
            throttlePolicy.EndpointWhitelist.Add("get:*.png");
            throttlePolicy.EndpointWhitelist.Add("get:*.jpg");
            throttlePolicy.EndpointWhitelist.Add("get:*.gif");
            throttlePolicy.EndpointWhitelist.Add("get:*.ico");

            config.MessageHandlers.Add(new CustomThrottlingHandler()
            {
                Policy = throttlePolicy,
                Repository = new CacheRepository()
            });

        }
    }
}
