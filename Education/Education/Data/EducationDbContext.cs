using Education.Areas.Identity.Models;
using Education.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Education.Data
{
    public class EducationDbContext : DbContext
    {

        public EducationDbContext(DbContextOptions<EducationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Education.Models.Task> Task { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>().ToTable("Project")
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Education.Models.Task>().ToTable("Task")
                .Property(m => m.Header).HasDefaultValue(false);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
             .Entries()
             .Where(e => e.Entity is BaseEntity && (
                 e.State == EntityState.Added
                 || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }

}
