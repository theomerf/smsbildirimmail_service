using Entities.Models;
using Repositories.Config;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base("DatabaseString")
        {
            Database.SetInitializer<RepositoryContext>(null);

            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
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

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            this.ChangeTracker.DetectChanges();
            return await base.SaveChangesAsync();
        }
    }
}
