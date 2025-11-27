using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Data;
using api.Enums;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => //figure out how this works
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApiContext>(options => 
    options.UseSqlite("Data Source=dbase.db")
    .UseSeeding((context, _) => //most likely all of this seeding code will be removed after, will look for better ways to populate data
    {
        var users = context.Set<User>();
        var services = context.Set<Service>();
        var salons = context.Set<Salon>();
        if (!users.Any())
        {
            for (int i = 0; i < 5; i++)
            {
                users.Add(new User
                {
                   Username = $"user{i}",
                   Password = $"pass{i}",
                   Email = $"mail{i}@gmail.com"
                });

                services.Add(new Service
                {
                   Name = $"service{i}"
                });

                salons.Add(new Salon
                {
                    Name = $"salon{i}",
                    City = api.Enums.City.Kaunas,
                    Address = $"Street {i}"
                });
            }
            context.SaveChanges();
        }
    })
    .UseAsyncSeeding(async (context, _, cancelToken) =>
    {
        var users = context.Set<User>();
        var services = context.Set<Service>();
        var salons = context.Set<Salon>();
        if (!await users.AnyAsync())
        {
            for (int i = 0; i < 5; i++)
            {
                users.Add(new User
                {
                   Username =  $"user{i}",
                   Password = $"pass{i}",
                   Email = $"mail{i}@gmail.com"
                });
                
                services.Add(new Service
                {
                   Name = $"service{i}"
                });

                salons.Add(new Salon
                {
                    Name = $"salon{i}",
                    City = City.Kaunas,
                    Address = $"Street {i}"
                });
            }
            await context.SaveChangesAsync(cancelToken);
        }
    }));

var app = builder.Build();

await using (var serviceScope = app.Services.CreateAsyncScope())
using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

//this is for wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
