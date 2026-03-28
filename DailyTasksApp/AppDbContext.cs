using Microsoft.EntityFrameworkCore;
using DailyTasksApp.Models;

namespace DailyTasksApp;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<SingleTask> SingleTaskTable { get; set; }
    public DbSet<DailyTasks> DailyTasksTable { get; set; }
    public DbSet<WeeklyTasks> WeeklyTasksTable { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SingleTask>().ToTable("SingleTasks");
        modelBuilder.Entity<DailyTasks>().ToTable("DailyTasks");
        modelBuilder.Entity<WeeklyTasks>().ToTable("WeeklyTasks");
    }
}