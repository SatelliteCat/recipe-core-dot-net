using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using recipe_core_dotnet.common;
using recipe_core_dotnet.common.Services.auth;
using recipe_core_dotnet.common.Services.users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RecipeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddControllers();

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        string? passwordHashSalt = builder.Configuration.GetValue<string>("PasswordHashSalt");

        if (string.IsNullOrEmpty(passwordHashSalt))
        {
            throw new Exception("Not found password hash salt.");
        }

        string? issuer = builder.Configuration.GetValue<string>("JwtTokenIssuer");

        if (string.IsNullOrEmpty(issuer))
        {
            throw new Exception("Not found JWT issuer.");
        }

        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = "audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(passwordHashSalt!))
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("TokenBucketPolicy", limiterOptions =>
    {
        limiterOptions.TokenLimit = 4;
        limiterOptions.TokensPerPeriod = 4;
        limiterOptions.QueueLimit = 2;
        limiterOptions.AutoReplenishment = true;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
    }).RejectionStatusCode = 429;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

// app.UseHttpsRedirection();
app.UseAuthorization();
// app.UseAuthentication();

app.MapControllers();

app.Run();