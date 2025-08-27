using System;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ISmsService
    {
        Task SendSmsAsync(Guid id);
    }
}
