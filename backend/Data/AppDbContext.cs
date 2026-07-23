using Microsoft.EntityFrameworkCore;
using WizardFormApi.Models;

namespace WizardFormApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<FormDefinition> Forms => Set<FormDefinition>();
    public DbSet<WorkflowDefinition> WorkflowDefinitions => Set<WorkflowDefinition>();
    public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();
    public DbSet<ProcessInstance> Processes => Set<ProcessInstance>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<ProcessHistory> ProcessHistories => Set<ProcessHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Department ──
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Roles)
            .WithOne(r => r.Department)
            .HasForeignKey(r => r.DepartmentId);

        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Code)
            .IsUnique();

        // ── Role ──
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Code)
            .IsUnique();

        // ── User ──
        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany()
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // ── FormDefinition ──
        modelBuilder.Entity<FormDefinition>()
            .HasOne(f => f.CreatedByUser)
            .WithMany()
            .HasForeignKey(f => f.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // ── WorkflowDefinition ──
        modelBuilder.Entity<WorkflowDefinition>()
            .HasMany(w => w.Steps)
            .WithOne(s => s.WorkflowDefinition)
            .HasForeignKey(s => s.WorkflowDefinitionId);

        modelBuilder.Entity<WorkflowDefinition>()
            .HasMany(w => w.Instances)
            .WithOne(p => p.WorkflowDefinition)
            .HasForeignKey(p => p.WorkflowDefinitionId);

        modelBuilder.Entity<WorkflowDefinition>()
            .HasOne(w => w.FormTemplate)
            .WithMany()
            .HasForeignKey(w => w.FormTemplateId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkflowDefinition>()
            .HasIndex(w => w.Code)
            .IsUnique();

        // ── WorkflowStep ──
        modelBuilder.Entity<WorkflowStep>()
            .HasOne(s => s.AssignedRole)
            .WithMany()
            .HasForeignKey(s => s.AssignedRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── ProcessInstance ──
        modelBuilder.Entity<ProcessInstance>()
            .HasOne(p => p.Form)
            .WithMany()
            .HasForeignKey(p => p.FormId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ProcessInstance>()
            .HasOne(p => p.CreatedByUser)
            .WithMany()
            .HasForeignKey(p => p.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProcessInstance>()
            .HasMany(p => p.WorkItems)
            .WithOne(w => w.Process)
            .HasForeignKey(w => w.ProcessInstanceId);

        modelBuilder.Entity<ProcessInstance>()
            .HasMany(p => p.History)
            .WithOne(h => h.Process)
            .HasForeignKey(h => h.ProcessInstanceId);

        // ── WorkItem ──
        modelBuilder.Entity<WorkItem>()
            .HasOne(w => w.WorkflowStep)
            .WithMany()
            .HasForeignKey(w => w.WorkflowStepId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(w => w.AssignedToUser)
            .WithMany()
            .HasForeignKey(w => w.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(w => w.AssignedToRole)
            .WithMany()
            .HasForeignKey(w => w.AssignedToRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(w => w.CompletedByUser)
            .WithMany()
            .HasForeignKey(w => w.CompletedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── ProcessHistory ──
        modelBuilder.Entity<ProcessHistory>()
            .HasOne(h => h.PerformedByUser)
            .WithMany()
            .HasForeignKey(h => h.PerformedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
