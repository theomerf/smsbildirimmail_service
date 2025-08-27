using Entities.Models;
using System.Data.Entity.ModelConfiguration;

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
