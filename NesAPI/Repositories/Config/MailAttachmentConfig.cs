using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public class MailAttachmentConfig : EntityTypeConfiguration<MailAttachment>
    {
        public MailAttachmentConfig() 
        {
            ToTable("MailAttachments")
                .HasKey(ma => ma.Id);

            Property(ma => ma.FileName)
                .IsRequired();

            Property(ma => ma.ContentType)
                .IsRequired();

            Property(ma => ma.FilePath)
                .IsRequired();

            HasRequired(ma => ma.Mail)
                .WithMany(m => m.Attachments);
        }
    }
}
