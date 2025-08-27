using System;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IMailService
    {
        Task SendMailAsync(Guid id);
    }
}
