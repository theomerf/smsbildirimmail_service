using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRequestRepository : IRepositoryBase<Request>
    {
        Task<Dictionary<Guid, RequestType>> GetPendingRequests();
        Task UpdateStatusAsync(Guid requestId, Status status);
        Task<Dictionary<Guid, Status>> GetRequestByIdAsync(Guid requestId);
    }
}
