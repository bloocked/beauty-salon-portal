using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(u =>
        {
            u.HasIndex(u => new { u.Email, u.Username }) //seperate 
            .IsUnique();
        });
    }
}