using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public class SmsConfig : EntityTypeConfiguration<SmsRequest>
    {
        public SmsConfig() 
        {
            ToTable("SmsRequests")
                .HasKey(s => s.Id);

            Property(s => s.Message)
                .IsRequired()
                .HasMaxLength(200);

            Property(s => s.From)
                .IsRequired();

            Property(s => s.To)
                .IsRequired();
        }
    }
}
