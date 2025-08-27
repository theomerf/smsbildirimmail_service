using Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace Repositories.Config
{
    public class MailConfig : EntityTypeConfiguration<MailRequest>
    {
        public MailConfig() 
        {
            ToTable("MailRequests")
                .HasKey(m => m.Id);

            Property(m => m.Subject)
                .IsRequired()
                .HasMaxLength(100);

            Property(m => m.Body)
                .IsRequired();

            Property(m => m.From)
                .IsRequired();

            Property(m => m.To)
                .IsRequired();

            HasMany(m => m.Attachments);
        } 
    }
}
