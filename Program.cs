using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Repository;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = "_allowedOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(

    options =>
    {
        options.AddPolicy(
            allowedOrigins,
            // allow all origins
            policy =>
                {
                    policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                }
            );
    }
    
);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("log/logger.txt", rollingInterval: RollingInterval.Infinite)
    .CreateLogger();

// Add DbContext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISessionsRepository, SessionsRepository>();
builder.Services.AddScoped<ITranslationsRepository, TranslationsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseRouting();

if (app.Environment.IsDevelopment() != true)
{
    app.UseCors(allowedOrigins);
}



app.UseAuthorization();

app.MapControllers();

app.Run();
