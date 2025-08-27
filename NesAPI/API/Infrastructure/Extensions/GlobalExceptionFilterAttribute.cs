using Entities.ErrorModel;
using Services.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace API.Infrastructure.Extensions
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILoggerService _logger;

        public GlobalExceptionFilterAttribute(ILoggerService logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.LogError($"Controller'da bir hata oluştu: {context.Exception}");

            var errorDetails = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = context.Exception.Message
            };

            context.Response = context.Request.CreateResponse(
                HttpStatusCode.InternalServerError,
                errorDetails
            );

            base.OnException(context);
        }
    }
}