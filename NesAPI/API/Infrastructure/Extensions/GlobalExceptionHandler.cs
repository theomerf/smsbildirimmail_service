using Entities.ErrorModel;
using Services.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Infrastructure.Handlers
{
    public class GlobalExceptionHandler : DelegatingHandler
    {
        private readonly ILoggerService _logger;

        public GlobalExceptionHandler(ILoggerService logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bir hata oluştu: {ex}");

                var errorDetails = new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"{ex}"
                };

                return request.CreateResponse(HttpStatusCode.InternalServerError, errorDetails);
            }
        }
    }
}
