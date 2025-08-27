using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
