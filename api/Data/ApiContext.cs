using api.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;

namespace api.Data;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Specialist> Specialists { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<SpecialistService> SpecialistServices { get; set; }

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

            s.Property(s => s.City)
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<City>(v));
        });

        modelBuilder.Entity<Service>(s =>
        {
            s.HasIndex(s => s.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Specialist>(s =>
        {
            s.HasOne(s => s.User)
                .WithOne(u => u.Specialist)
                .HasForeignKey<Specialist>(s => s.UserId);

            s.HasOne(s => s.Salon)
                .WithMany(s => s.Specialists)
                .HasForeignKey(s => s.SalonId);
        });

        modelBuilder.Entity<SpecialistService>(s =>
        {
            s.HasOne(ss => ss.Service)
                .WithOne(s => s.SpecialistService)
                .HasForeignKey<SpecialistService>(ss => ss.ServiceId);

            s.HasOne(ss => ss.Specialist)
                .WithMany(s => s.Services)
                .HasForeignKey(ss => ss.SpecialistId);
        });
    }
}