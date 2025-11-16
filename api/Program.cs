using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Data;
using Microsoft.Extensions.Options;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApiContext>(options => 
    options.UseSqlite("Data Source=dbase.db")
    .UseSeeding((context, _ ) =>
    {
        var users = context.Set<User>();
        if (!users.Any())
        {
            for (int i = 0; i < 5; i++)
            {
                users.Add(new User
                {
                   Username =  $"user{i}",
                   Password = $"pass{i}",
                   Email = $"mail{i}@gmail.com"
                });
            }
            context.SaveChanges();
        }
    })
    .UseAsyncSeeding(async (context, _, cancelToken) =>
    {
        var users = context.Set<User>();
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
