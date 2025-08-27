using API.Infrastructure.Extensions;
using Services.Contracts;
using Services.Dtos;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    public class MailController : ApiController
    {
        private readonly IServiceManager _manager;

        public MailController(IServiceManager manager)
        {
            _manager = manager;
        }

        [ApiKeyAuth]
        [HttpPost]
        public async Task<IHttpActionResult> Send([FromBody]MailDto mailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _manager.MailService.CreateMailAsync(mailDto);
            return StatusCode(HttpStatusCode.Created);
        }
    }
}