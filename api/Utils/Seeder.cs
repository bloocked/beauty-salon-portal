using api.Data;
using api.Enums;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Utils;

public static class Seeder
{
    /// <summary>
    /// Async seeds the db tables (Users, Services, Salons, Specialists)
    /// </summary>
    /// <param name="context">The db context</param>
    /// <param name="ct"></param>
    /// <returns>A task to be awaited</returns>
    public static async Task SeedAsync(ApiDbContext context, CancellationToken ct = default)
    {
        if (await context.Users.AnyAsync(ct)) return; //if seeded already

        for (int i = 0; i < 10; i++)
        {
            context.Users.Add(new User
            {
                Username = $"user{i}",
                PasswordHash = Hasher.Hash($"pass{i}"),
                Email = $"mail{i}@gmail.com"
            });

            context.Services.Add(new Service
            {
                Name = $"service{i}"
            });

            context.Salons.Add(new Salon
            {
                Name = $"salon{i}",
                City = i % 2 == 0 ? City.Kaunas : City.Vilnius, // based ternary
                Address = $"Street {i}"
            });
        }
        await context.SaveChangesAsync(ct);
        
        var savedUsers = await context.Users.ToListAsync(ct);
        var savedSalons = await context.Salons.ToListAsync(ct);
        var savedServices = await context.Services.ToListAsync(ct);

        for (int i = 0; i < savedUsers.Count; i++)
        {
            if (i % 3 == 0)
            {
                context.Specialists.Add(new Specialist
                {
                    UserId = savedUsers[i].Id,
                    SalonId = savedSalons[i].Id
                });
            }
        }

        await context.SaveChangesAsync(ct);

        Random random = new Random();

        var specialistList = await context.Specialists.ToListAsync(ct);

        foreach(Specialist s in specialistList)
        {
            List<Service> shuffledServices = savedServices.OrderBy(s => random.Next()).ToList();

            foreach (Service service in shuffledServices.Take(3))
            {
                context.SpecialistServices.Add(new SpecialistService
                {
                    SpecialistId = s.Id,
                    ServiceId = service.Id,
                    Cost = random.NextDouble() * 100,
                    Duration = TimeSpan.FromMinutes(random.NextInt64(6) * 15)
                });
            }
        }

        await context.SaveChangesAsync(ct);
    }
}