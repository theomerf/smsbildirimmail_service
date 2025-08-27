using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MailRepository : RepositoryBase<MailRequest>, IMailRepository
    {
        public MailRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateMail(MailRequest mail)
        {
            Create(mail);
        }
    }
}
