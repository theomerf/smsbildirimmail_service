using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IStatRepository : IRepositoryBase<Stat>
    {
        Task<IEnumerable<Stat>> GetAllStatsAsync();
    }
}
