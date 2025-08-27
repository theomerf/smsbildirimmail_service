using Microsoft.EntityFrameworkCore;
using NesInterface.Models;

namespace NesInterface
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
