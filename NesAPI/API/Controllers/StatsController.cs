using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class StatsController : ApiController
    {
        private readonly IServiceManager _manager;

        public StatsController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var stats = await _manager.StatsService.GetAllStatsAsync();

            return Json(stats);
        }
    }
}