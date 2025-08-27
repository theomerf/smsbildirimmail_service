using Entities.Models;
using Repositories.Contracts;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Repositories
{
    public class SmsRepository : RepositoryBase<SmsRequest>, ISmsRepository
    {
        public SmsRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<SmsRequest> GetSmsById(Guid id)
        {
            var sms = await FindByCondition(s => s.Id == id, false)
                .FirstOrDefaultAsync();

            return sms;
        }
    }
}
