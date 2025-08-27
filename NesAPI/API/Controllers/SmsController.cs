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
    public class SmsController : ApiController
    {
        private readonly IServiceManager _manager;

        public SmsController(IServiceManager manager)
        {
            _manager = manager;
        }

        [ApiKeyAuth]
        [HttpPost]
        public async Task<IHttpActionResult> Send([FromBody] SmsDto smsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _manager.SmsService.CreateSmsAsync(smsDto);
            return StatusCode(HttpStatusCode.Created);
        }

    }
}