using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SunriseContext : DbContext
    {
        public SunriseContext(DbContextOptions<SunriseContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Hazard> Hazards { get; set; }
        public DbSet<HazardSWMS> HazardSWMSs { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobAfterPhoto> JobAfterPhotos { get; set; }
        public DbSet<JobBeforePhoto> JobBeforePhotos { get; set; }
        public DbSet<JobChangeRecord> JobChangeRecords { get; set; }
        public DbSet<JobHazard> JobHazards { get; set; }
        public DbSet<JobLog> JobLogs { get; set; }
        public DbSet<JobNote> JobNotes { get; set; }
        public DbSet<JobTask> JobTasks { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteDocument> NoteDocuments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SafeWorkMethodStatement> SafeWorkMethodStatements { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffRole> StaffRoles { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<JobLine> JobLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            // {
            //     foreach (var property in entityType.GetProperties())
            //     {
            //         if (property.ClrType == typeof(bool))
            //         {
            //             property.SetValueConverter(new BoolToIntConverter());
            //         }
            //     }
            // }
            // modelBuilder.Entity<Job>(entity =>
            // {
            //     entity.HasMany(t => t.BeforePhotos)
            //         .WithOne(b => b.Job)
            //         .HasForeignKey(b => b.JobId);

            //     entity.HasMany(t => t.AfterPhotos)
            //         .WithOne(a => a.Job)
            //         .HasForeignKey(a => a.JobId);

            //     entity.HasMany(t => t.RelatedNotes)
            //         .WithOne(r => r.Job)
            //         .HasForeignKey(r => r.JobId);
                    
            //     entity.HasMany(t => t.JobHazards)
            //         .WithOne(h => h.Job)
            //         .HasForeignKey(h => h.JobId);
                    
            //     entity.HasMany(t => t.JobTasks)
            //         .WithOne(t => t.Job)
            //         .HasForeignKey(t => t.JobId);
                    
            //     entity.HasMany(t => t.JobLogs)
            //         .WithOne(j => j.Job)
            //         .HasForeignKey(j => j.JobId);
            // });
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasMany(j => j.JobLines)
                    .WithOne(jl => jl.Job)
                    .HasForeignKey(jl => jl.JobId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasMany(t => t.CustomerAddresses)
                    .WithOne(c => c.Customer)
                    .HasForeignKey(a => a.CustomerId);
            });
            
            modelBuilder.Entity<Hazard>(entity =>
            {
                entity.HasMany(t => t.SafeWorkMethodStatements)
                    .WithOne(s => s.Hazard)
                    .HasForeignKey(h => h.HazardId);
            });
            
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasMany(t => t.NoteDocuments)
                    .WithOne(n => n.Note)
                    .HasForeignKey(n => n.NoteId);
            });
            
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasMany(t => t.StaffRoles)
                    .WithOne(s => s.Staff)
                    .HasForeignKey(s => s.StaffId);
            });
            
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasOne(t => t.UploadedByStaff)
                    .WithMany()
                    .HasForeignKey(s => s.UploadedByStaffId);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(t => new { t.CustomerId, t.AddressId });
            });

            modelBuilder.Entity<HazardSWMS>(entity =>
            {
                entity.HasKey(t => new { t.HazardId, t.SafeWorkMethodStatementId });
            });

            modelBuilder.Entity<JobAfterPhoto>(entity =>
            {
                entity.HasKey(t => new { t.JobId, t.DocumentId });
            });

            modelBuilder.Entity<JobBeforePhoto>(entity =>
            {
                entity.HasKey(t => new { t.JobId, t.DocumentId });
            });

            modelBuilder.Entity<JobHazard>(entity =>
            {
                entity.HasKey(t => new { t.HazardId, t.JobId });
            });

            modelBuilder.Entity<JobLog>(entity =>
            {
                entity.HasKey(t => new { t.JobId, t.LogId });
            });

            modelBuilder.Entity<JobNote>(entity =>
            {
                entity.HasKey(t => new { t.JobId, t.NoteId });
            });

            modelBuilder.Entity<JobTask>(entity =>
            {
                entity.HasKey(t => new { t.JobId, t.TaskId });
            });

            modelBuilder.Entity<NoteDocument>(entity =>
            {
                entity.HasKey(t => new { t.NoteId, t.DocumentId });
            });

            modelBuilder.Entity<StaffRole>(entity =>
            {
                entity.HasKey(t => new { t.StaffId, t.RoleId });
            });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}