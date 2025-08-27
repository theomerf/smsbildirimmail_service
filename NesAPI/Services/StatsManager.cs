using AutoMapper;
using Repositories.Contracts;
using Services.Contracts;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StatsManager : IStatsService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public StatsManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StatDto>> GetAllStatsAsync()
        {
            var stats = await _manager.Stat.GetAllStatsAsync();
            var statsDto = _mapper.Map<IEnumerable<StatDto>>(stats);

            return statsDto;
        }
    }
}
