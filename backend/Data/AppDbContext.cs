using Microsoft.EntityFrameworkCore;
using WizardFormApi.Models;

namespace WizardFormApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<FormDefinition> Forms => Set<FormDefinition>();
    public DbSet<ProcessInstance> Processes => Set<ProcessInstance>();
    public DbSet<ProcessHistory> ProcessHistories => Set<ProcessHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcessInstance>()
            .HasMany(p => p.History)
            .WithOne(h => h.Process)
            .HasForeignKey(h => h.ProcessInstanceId);

        modelBuilder.Entity<ProcessInstance>()
            .HasOne(p => p.Form)
            .WithMany()
            .HasForeignKey(p => p.FormId);
    }
}