using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using PersonalFinancePlatform.Infrastructure.Persistence.Repository;
using PersonalFinancePlatform.Infrastructure.Security.PasswordHasher;
using PersonalFinancePlatform.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//DB connection string
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

//Infra/Implementation/DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
