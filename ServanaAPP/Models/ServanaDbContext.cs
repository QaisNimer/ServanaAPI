using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace ServanaAPP.Models
{
    public class ServanaDbContext:DbContext
    {
        public ServanaDbContext(DbContextOptions options) : base(options)
        { 
        
        }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rating>()
            .HasIndex(r => r.RequestID)
            .IsUnique();

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Client)
                .WithMany(u => u.RatingsGiven)
                .HasForeignKey(r => r.ClientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Worker)
                .WithMany(u => u.RatingsReceived)
                .HasForeignKey(r => r.WorkerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobRequest>()
            .HasOne(j => j.Client)
            .WithMany(u => u.JobRequestsSent)
            .HasForeignKey(j => j.ClientID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobRequest>()
                .HasOne(j => j.Worker)
                .WithMany(u => u.JobRequestsReceived)
                .HasForeignKey(j => j.WorkerID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobRequest>()
    .HasOne(j => j.Payment)
    .WithOne(p => p.JobRequest)
    .HasForeignKey<Payment>(p => p.RequestID)
    .OnDelete(DeleteBehavior.Cascade); // or Restrict, as needed

            modelBuilder.Entity<JobRequest>()
    .HasOne(j => j.Rating)
    .WithOne(r => r.JobRequest)
    .HasForeignKey<Rating>(r => r.RequestID)
    .OnDelete(DeleteBehavior.Cascade); // or Restrict

            modelBuilder.Entity<JobRequest>()
    .HasOne(j => j.WorkSession)
    .WithOne(ws => ws.JobRequest)
    .HasForeignKey<WorkSession>(ws => ws.RequestID)
    .OnDelete(DeleteBehavior.Cascade); // or Restrict

            modelBuilder.Entity<JobRequest>()
    .HasKey(j => j.RequestID);
            modelBuilder.Entity<WorkSession>()
    .HasKey(ws => ws.SessionID);

        }
    }
}
