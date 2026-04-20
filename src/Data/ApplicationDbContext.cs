using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.CategoryNavigation)
            .WithMany(c => c.Lessons)
            .HasForeignKey(l => l.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Warehouse Fundamentals" },
            new Category { Id = 2, Name = "Systems and Data" },
            new Category { Id = 3, Name = "Design and Experience" });

        modelBuilder.Entity<Lesson>().HasData(
            new Lesson
            {
                Id = 1,
                Title = "Inventory Lifecycle",
                Content = "Understand inventory flow from procurement to storage, picking, shipping, and returns.",
                Category = "Warehouse Fundamentals",
                CategoryId = 1
            },
            new Lesson
            {
                Id = 2,
                Title = "Warehouse Operations",
                Content = "Learn receiving, put-away strategies, cycle counting, and safety-driven daily operations.",
                Category = "Warehouse Fundamentals",
                CategoryId = 1
            },
            new Lesson
            {
                Id = 3,
                Title = "Logistics Basics",
                Content = "Explore transportation planning, lead times, service levels, and fulfillment constraints.",
                Category = "Warehouse Fundamentals",
                CategoryId = 1
            },
            new Lesson
            {
                Id = 4,
                Title = "Database Relations",
                Content = "See how relational modeling connects lessons, categories, and users with proper integrity.",
                Category = "Systems and Data",
                CategoryId = 2
            },
            new Lesson
            {
                Id = 5,
                Title = "UI Design Basics",
                Content = "Review layout composition, readability, and navigation patterns for operational software.",
                Category = "Design and Experience",
                CategoryId = 3
            });
    }
}
