using System.Text;
using E_commerce_application.Interfaces;
using E_commerce_infrastructure.Cloudinary;
using E_commerce_infrastructure.Email;
using E_commerce_infrastructure.Identities;
using E_commerce_infrastructure.Redis;
using E_commerce_infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace E_commerce_infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
                    sql.MigrationsHistoryTable("__ApplicationMigrationsHistory", "app"))
                .EnableSensitiveDataLogging();
        });

        services.AddDbContext<AppIdentityDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
                    sql.MigrationsHistoryTable("__IdentityMigrationsHistory", "identity"))
                .EnableSensitiveDataLogging();
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();
            // .AddDefaultTokenProviders();
            
            
            //
            // services.AddAuthorization();
            // services.AddHttpContextAccessor();    
//JWT
        services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));
        var jwtSettings = config.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                          ?? throw new InvalidOperationException($"Configuration section '{JwtSettings.SectionName}' is missing.");

        if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || jwtSettings.SecretKey.Length < 32)
            throw new InvalidOperationException("Jwt:Secret must be at least 32 characters.");
    
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
    

//cloudinary register

        services.Configure<CloudinarySettings>(
            config.GetSection(CloudinarySettings.SectionName)
        );
        
        
        
//Email register not finished      

        services.Configure<EmailSettings>(
            config.GetSection(EmailSettings.SectionName));
        
        var emailSettings = config.GetSection(EmailSettings.SectionName).Get<EmailSettings>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{EmailSettings.SectionName}' is missing.");
        
        services
            .AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
            .AddSmtpSender(emailSettings.Host, emailSettings.Port);
        
        
        services.AddScoped<
            IEmailVerificationCodeStore,
            EmailVerificationCodeStore>();
        
        
//redis part not finished
        
        services.Configure<RedisSettings>(
            config.GetSection(RedisSettings.SectionName));
        
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var settings = sp
                .GetRequiredService<IOptions<RedisSettings>>()
                .Value;

            var configuration = new ConfigurationOptions
            {
                EndPoints =
                {
                    { settings.Endpoint, settings.Port }
                },
                User = settings.User,
                Password = settings.Password
            };

            return ConnectionMultiplexer.Connect(configuration);
        });
        
        
        
//seed data 

        services.AddScoped<IDataSeeder, IdentitySeeder>();
        services.AddScoped<IDataSeeder, BrandSeeder>();
        services.AddScoped<IDataSeeder, CategorySeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<IDataSeeder, DeliveryMethodSeeder>();
        services.AddScoped<DatabaseSeeder>();
        
        


        return services;
    }
    
}