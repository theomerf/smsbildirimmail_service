using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities.Models
{
    public class Request
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public RequestType Type { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public virtual NotificationRequest NotificationRequest { get; set; }
        public virtual MailRequest MailRequest { get; set; }
        public virtual SmsRequest SmsRequest { get; set; }
        public bool Validate(RequestType type, out string error)
        {
            if (!Enum.IsDefined(typeof(RequestType), type))
            {
                error = "Geçersiz RequestType değeri.";
                return false;
            }
            error = null;
            return true;
        }

    }

    public enum RequestType
    {
        Mail = 1,
        Notification = 2,
        Sms = 3,
    }

    public enum Status
    {
        Pending = 0,
        Processing = 1,
        Done = 2,
        Error = 3,
    }
}
