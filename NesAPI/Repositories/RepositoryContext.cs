using Entities.Models;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base("name=DatabaseString")
        {
        }
        public DbSet<Request> Requests { get; set; }
        public DbSet<NotificationRequest> NotificationRequests { get; set; }
        public DbSet<SmsRequest> SmsRequests { get; set; }
        public DbSet<MailRequest> MailRequests { get; set; }
        public DbSet<MailAttachment> MailAttachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new NotificationConfig());
            modelBuilder.Configurations.Add(new MailConfig());
            modelBuilder.Configurations.Add(new SmsConfig());
            modelBuilder.Configurations.Add(new RequestConfig());
            modelBuilder.Configurations.Add(new MailAttachmentConfig());
        }
    }
}
