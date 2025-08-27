using Entities.Models;
using System;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataWatcherService
    {
        void StartWatching();
        void StopWatching();
    }
}
