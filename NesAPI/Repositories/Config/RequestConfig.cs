using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public class RequestConfig : EntityTypeConfiguration<Request>
    {
        public RequestConfig()
        {
            ToTable("Requests")
                .HasKey(r => r.Id);

            Property(r => r.Type)
                .IsRequired();

            HasOptional(r => r.MailRequest)
                .WithRequired(m => m.Request);

            HasOptional(r => r.SmsRequest)
                .WithRequired(s => s.Request);

            HasOptional(r => r.NotificationRequest)
                .WithRequired(n => n.Request); 
        }
    }
}