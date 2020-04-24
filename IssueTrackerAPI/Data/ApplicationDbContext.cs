using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IssueTrackerAPI.Models;

namespace IssueTrackerAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Severity> Severities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Assignee> Assignees { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Issue>().ToTable(nameof(Issue));
            builder.Entity<Person>().ToTable(nameof(Person));
            builder.Entity<Project>().ToTable(nameof(Project));
            builder.Entity<Severity>().ToTable(nameof(Severity));
            builder.Entity<Status>().ToTable(nameof(Status));
            builder.Entity<Assignee>().ToTable(nameof(Assignee));
            builder.Entity<ProjectMember>().ToTable(nameof(ProjectMember));

            builder.Entity<Assignee>()
                .HasKey(a => new { a.IssueId, a.PersonId });

            builder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.PersonId });

            builder.Entity<Assignee>()
                 .HasOne<Person>(e => e.Person)
                 .WithMany(e => e.Assignees)
                 .HasForeignKey(e => e.PersonId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
