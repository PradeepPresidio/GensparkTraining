using TeamColabApp.Models;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Contexts
{
    public class TeamColabAppContext : DbContext
    {
        public TeamColabAppContext(DbContextOptions<TeamColabAppContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AppFile> AppFiles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //         modelBuilder.Entity<Project>()
            // .Ignore(p => p.TeamLeader); 
            modelBuilder.Entity<User>()
    .HasMany(u => u.TeamLedProjects)
    .WithOne(p => p.TeamLeader)
    .HasForeignKey(p => p.TeamLeaderId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithMany(p => p.TeamMembers)
                .UsingEntity(j => j.ToTable("UserProjects"));
            modelBuilder.Entity<User>()
                .HasMany(u => u.ProjectTasks)
                .WithMany(t => t.AssignedUsers)
                .UsingEntity(j => j.ToTable("UserProjectTasks"));
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserFiles)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.TeamLeader)
                .WithMany(u => u.TeamLedProjects)
                .HasForeignKey(p => p.TeamLeaderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectTasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Notifications)
                .WithOne(n => n.Project)
                .HasForeignKey(n => n.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectFiles)
                .WithOne(f => f.Project)
                .HasForeignKey(f => f.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectTask>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.ProjectTask)
                .HasForeignKey(c => c.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectTask>()
                .HasMany(t => t.Notifications)
                .WithOne(n => n.ProjectTask)
                .HasForeignKey(n => n.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProjectTask>()
                .HasMany(t => t.ProjectTaskFiles)
                .WithOne(f => f.ProjectTask)
                .HasForeignKey(f => f.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
            // modelBuilder.Entity<UserToken>()
            //     .HasOne(ut => ut.User)
            //     .WithMany(u => u.Tokens)
            //     .HasForeignKey(ut => ut.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}