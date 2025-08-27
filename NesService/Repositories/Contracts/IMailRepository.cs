using Entities.Models;
using System;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IMailRepository : IRepositoryBase<MailRequest>
    {
        Task<MailRequest> GetMailById(Guid id);
    }
}
