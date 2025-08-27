using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class StatDto
    {
        public String To;
        public int SuccessCount;
        public int FailureCount;
        public int MailCount;
        public int SmsCount;
        public int NotificationCount;
    }
}
