
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Authentication;

namespace SurveyBasket;

public  static class DependancyInjection
{

    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApiConfig()
                .AddFluentValidationConfig()
                .AddRegisteredConfig()
                .AddAuthConfig();
        

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;

    }

    public static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        return services;
    }
    public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
    public static IServiceCollection AddRegisteredConfig(this IServiceCollection services)
    {
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
    public static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddSingleton<IJwtProvidor, JwtProvidor>();
        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
