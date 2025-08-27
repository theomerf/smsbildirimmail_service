using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<Dictionary<Guid, RequestType>> GetPendingRequests()
        {
            var requests = await FindByCondition(r => r.Status == 0, false)
                .Select(r => new { r.Id, r.Type })
                .ToDictionaryAsync(r => r.Id, r => r.Type);

            return requests;
        }

        public async Task UpdateStatusAsync(Guid requestId, Status status)
        {
            var request = await FindByCondition(r => r.Id == requestId, true)
                .FirstOrDefaultAsync();

            if (request != null)
            {
                request.Status = status;
                request.UpdatedAt = DateTime.Now;
            }
        }

        public async Task<Dictionary<Guid, Status>> GetRequestByIdAsync(Guid requestId)
        {
            var request = await FindByCondition(r => r.Id == requestId, false)
                .Select(r => new { r.Id, r.Status })
                .ToDictionaryAsync(r => r.Id, r => r.Status);

            return request;
        }
    }
}
