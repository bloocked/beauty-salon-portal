using api.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace api.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Specialist> Specialists { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<SpecialistService> SpecialistServices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(u =>
        {
            u.HasIndex(u => u.Email)
                .IsUnique();

            u.HasIndex(u => u.Username)
                .IsUnique();

            u.Property(u => u.Username)
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            u.Property(u => u.Email)
                .HasColumnType("TEXT")
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Salon>(s =>
        {
            s.HasIndex(s => s.Name)
                .IsUnique();

            s.HasIndex(s => s.Address)
                .IsUnique();

            s.Property(s => s.City)
                .HasConversion<string>()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            s.Property(s => s.Name)
                .HasColumnType("TEXT")
                .HasMaxLength(100);

            s.Property(s => s.Address)
                .HasColumnType("TEXT")
                .HasMaxLength(200);
        });

        modelBuilder.Entity<Service>(s =>
        {
            s.HasIndex(s => s.Name)
                .IsUnique();

            s.Property(s => s.Name)
                .HasColumnType("TEXT")
                .HasMaxLength(100);
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
            s.HasIndex(ss => new { ss.SpecialistId, ss.ServiceId })
                .IsUnique();

            s.HasOne(ss => ss.Service)
                .WithMany(s => s.SpecialistServices)
                .HasForeignKey(ss => ss.ServiceId);

            s.HasOne(ss => ss.Specialist)
                .WithMany(s => s.SpecialistServices)
                .HasForeignKey(ss => ss.SpecialistId);

            s.Property(ss => ss.Cost)
                .HasColumnType("NUMERIC(10,2)");

            s.Property(ss => ss.Duration)
                .HasColumnType("TEXT");
        });

       modelBuilder.Entity<Reservation>(r =>
       {
            r.HasIndex(r => new { r.SpecialistId, r.StartTime })
                .IsUnique();

            r.HasOne(r => r.SpecialistService)
                .WithMany(ss => ss.Reservations)
                .HasForeignKey(r => r.SpecialistServiceId);

            r.HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId);

            r.Property(r => r.StartTime)
                .HasColumnType("TEXT");

            r.Property(r => r.EndTime)
                .HasColumnType("TEXT");
       });

       modelBuilder.Entity<Admin>(a =>
       {
            a.HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId);
       });
    }
}