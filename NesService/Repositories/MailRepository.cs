using Entities.Models;
using Repositories.Contracts;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Repositories
{
    public class MailRepository : RepositoryBase<MailRequest>, IMailRepository
    {
        public MailRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<MailRequest> GetMailById(Guid id)
        {
            var mail = await FindByCondition(m => m.Id == id, false)
                .Include(m => m.Attachments)
                .FirstOrDefaultAsync();

            return mail;
        }
    }
}
