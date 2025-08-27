using API.Infrastructure.Extensions;
using Services.Contracts;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly IServiceManager _manager;

        public NotificationController(IServiceManager manager)
        {
            _manager = manager;
        }

        [ApiKeyAuth]
        [HttpPost]
        public async Task<IHttpActionResult> Send([FromBody] NotificationDto notificationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _manager.NotificationService.CreateNotificationAsync(notificationDto);
            return StatusCode(HttpStatusCode.Created);
        }
    }
}