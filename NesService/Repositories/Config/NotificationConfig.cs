using Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace Repositories.Config
{
    public class NotificationConfig : EntityTypeConfiguration<NotificationRequest>
    {
        public NotificationConfig() 
        {
            ToTable("NotificationRequests")
                .HasKey(n => n.Id);

            Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(100);

            Property(n => n.Body)
                .IsRequired();

            Property(n => n.To)
                .IsRequired();
        }
    }
}
