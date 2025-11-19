using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Salon> Salons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(u =>
        {
            u.HasIndex(u => u.Email)
                .IsUnique();
            u.HasIndex(u => u.Username)
                .IsUnique();
        });

        modelBuilder.Entity<Salon>(s =>
        {
            s.HasIndex(s => s.Name)
                .IsUnique();
            s.HasIndex(s => s.Address)
                .IsUnique();
        });
    }
}