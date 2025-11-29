using Microsoft.EntityFrameworkCore;
using api.Data;
using System.Text.Json.Serialization;
using api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => //figure out how this works
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApiDbContext>(options => 
    options.UseSqlite("Data Source=dbase.db")
    .UseAsyncSeeding(async (context, _, cancelToken) =>
    {
        await Seeder.SeedAsync((ApiDbContext)context, cancelToken);
    }));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //perhaps change to false
        ValidateAudience = true, //perhaps change to false
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Specialist", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Specialist") || context.User.IsInRole("Admin")));
});

var app = builder.Build();

await using (var serviceScope = app.Services.CreateAsyncScope())
using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApiDbContext>())
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
