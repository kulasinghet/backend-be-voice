using Be_My_Voice_Backend.Data;
using Be_My_Voice_Backend.Repository;
using Be_My_Voice_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = "_allowedOrigins";

// Add services to the container.

var key = builder.Configuration.GetValue<string>("AppSettings:Secret");

builder.Services.AddAuthentication(allowedOrigins =>
{
    allowedOrigins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    allowedOrigins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(
    allowedOrigins =>
    {
        allowedOrigins.RequireHttpsMetadata = false;
        allowedOrigins.SaveToken = true;
        allowedOrigins.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false

        };
    }
    );

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
builder.Services.AddScoped<IQuizRepository, QuizRepository>();

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


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
