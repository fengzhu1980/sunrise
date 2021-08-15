using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(j => j.JobCode).IsRequired();
            
            builder.HasOne(j => j.AssignedToStaff)
                .WithMany()
                .HasForeignKey(j => j.AssignedToStaffId);

            builder.HasMany(t => t.BeforePhotos)
                .WithOne(b => b.Job)
                .HasForeignKey(b => b.JobId);

            builder.HasMany(t => t.AfterPhotos)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobId);

            builder.HasMany(t => t.RelatedNotes)
                .WithOne(r => r.Job)
                .HasForeignKey(r => r.JobId);

            builder.HasMany(t => t.JobHazards)
                .WithOne(h => h.Job)
                .HasForeignKey(h => h.JobId);

            builder.HasMany(t => t.JobTasks)
                .WithOne(t => t.Job)
                .HasForeignKey(t => t.JobId);

            builder.HasMany(t => t.JobLogs)
                .WithOne(j => j.Job)
                .HasForeignKey(j => j.JobId);
        }
    }
}