using IceSync.Configurations;
using IceSync.Models;
using Microsoft.EntityFrameworkCore;

namespace IceSync.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {
        public DbSet<Workflow> Workflows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new WorkflowConfiguration());
        }
    }
}
