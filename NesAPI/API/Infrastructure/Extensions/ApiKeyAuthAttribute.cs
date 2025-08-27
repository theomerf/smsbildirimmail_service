using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Infrastructure.Extensions
{
    public class ApiKeyAuthAttribute : AuthorizationFilterAttribute
    {
        private const string ApiKeyHeaderName = "X-Api-Key";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            IEnumerable<string> apiKeyHeaders;
            if (!actionContext.Request.Headers.TryGetValues(ApiKeyHeaderName, out apiKeyHeaders) ||
                apiKeyHeaders == null || !apiKeyHeaders.Any())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "API key eksik");
                return;
            }

            var apiKey = apiKeyHeaders.First();
            var validApiKey = ConfigurationManager.AppSettings["ApiKey"];

            if (apiKey != validApiKey) 
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Geçersiz api keyi");
                return;
            }

            base.OnAuthorization(actionContext);
        }
    }
}