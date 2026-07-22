using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using PersonalFinancePlatform.Infrastructure.Persistence.Repository;
using PersonalFinancePlatform.Infrastructure.Security.PasswordHasher;
using PersonalFinancePlatform.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//DB connection string
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

//Infra/Implementation/DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

//MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Swagger/Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
