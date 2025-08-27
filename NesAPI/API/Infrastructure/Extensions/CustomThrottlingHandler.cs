using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebApiThrottle;

namespace API.Infrastructure.Extensions
{
    public class CustomThrottlingHandler : ThrottlingHandler
    {
        protected override Task<HttpResponseMessage> QuotaExceededResponse(HttpRequestMessage request, object content, HttpStatusCode responseCode, string retryAfter)
        {
            var message = new HttpResponseMessage(responseCode)
            {
                Content = new StringContent("Çok fazla istek gönderdiniz. Lütfen biraz bekleyin.")
            };
            message.Headers.Add("Retry-After", retryAfter);

            return Task.FromResult(message);
        }
    }
}