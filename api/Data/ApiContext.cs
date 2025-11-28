using Microsoft.AspNetCore.Identity;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api.Data;

public class ApiContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    //this gets inherited from identity
    //public DbSet<User> Users { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Specialist> Specialists { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<SpecialistService> SpecialistServices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); //identity needs this

        modelBuilder.Entity<User>(u =>
        {
            u.HasIndex(u => u.Email)
                .IsUnique();

            u.HasIndex(u => u.UserName)
                .IsUnique();
        });

        modelBuilder.Entity<Salon>(s =>
        {
            s.HasIndex(s => s.Name)
                .IsUnique();

            s.HasIndex(s => s.Address)
                .IsUnique();

            s.Property(s => s.City)
                .HasConversion<string>();
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
                .WithMany(s => s.SpecialistServices)
                .HasForeignKey(ss => ss.ServiceId);

            s.HasOne(ss => ss.Specialist)
                .WithMany(s => s.SpecialistServices)
                .HasForeignKey(ss => ss.SpecialistId);
        });

       modelBuilder.Entity<Reservation>(r =>
       {
            r.HasIndex(r => new { r.StartTime, r.SpecialistServiceId })
                .IsUnique();

            r.HasOne(r => r.SpecialistService)
                .WithMany(ss => ss.Reservations)
                .HasForeignKey(ss => ss.SpecialistServiceId);

            r.HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.UserId);
       });
    }
}