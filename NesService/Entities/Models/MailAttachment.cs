using System;

namespace Entities.Models
{
    public class MailAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MailId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public virtual MailRequest Mail { get; set; }
    }
}
