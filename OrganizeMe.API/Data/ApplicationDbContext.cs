using Microsoft.EntityFrameworkCore;
using OrganizeMe.API.Models;

namespace OrganizeMe.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId); 

        modelBuilder.Entity<User>()
            .HasMany(u => u.TodoItems)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        modelBuilder.Entity<TodoItem>()
            .HasKey(t => t.ItemId); 

        base.OnModelCreating(modelBuilder);
    }
}