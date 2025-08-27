using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SmsRepository : RepositoryBase<SmsRequest>, ISmsRepository
    {
        public SmsRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateSms(SmsRequest sms)
        {
            Create(sms);
        }
    }
}
