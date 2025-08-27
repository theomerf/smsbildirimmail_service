using Entities.Models;
using System;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ISmsRepository : IRepositoryBase<SmsRequest>
    {
        Task<SmsRequest> GetSmsById(Guid id);
    }
}
