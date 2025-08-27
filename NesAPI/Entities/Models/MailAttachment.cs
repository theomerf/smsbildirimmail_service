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

        public bool Validate(out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(FileName))
            {
                error = "'FileName' boş olamaz.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(FilePath))
            {
                error = "'FilePath' boş olamaz.";
                return false;
            }

            return true;
        }
    }
}
