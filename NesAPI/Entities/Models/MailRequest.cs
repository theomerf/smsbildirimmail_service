using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override bool Validate(out string error)
        {
            if (!base.Validate(out error)) return false;

            if (string.IsNullOrWhiteSpace(Subject))
            {
                error = "'Subject' boş olamaz.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Body))
            {
                error = "'Body' boş olamaz.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(From))
            {
                error = "'From' alanı boş olamaz.";
                return false;
            }

            return true;
        }
    }
}
