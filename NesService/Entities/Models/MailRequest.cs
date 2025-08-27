using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class MailRequest : RequestBase
    {
        public String Subject { get; set; }
        public String Cc { get; set; }
        public String Bcc { get; set; } 
        public String Body { get; set; }
        public bool IsBodyHtml { get; set; } = false;
        public ICollection<MailAttachment> Attachments { get; set; } = new List<MailAttachment>();
        public virtual Request Request { get; set; }
    }
}
