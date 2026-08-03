
namespace SurveyBasket;

public  static class DependancyInjection
{

    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApiConfig()
                .AddFluentValidationConfig()
                .AddRegisteredConfig()
                .AddAuthConfig(configuration);
        

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
    public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration _configuration)
    {
       // services.Configure<JwtOptions>(_configuration.GetSection(JwtOptions.SectionName));
       services.AddOptions<JwtOptions>()
                .Bind(_configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        var jwtSettings = _configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddScoped<IJwtProvidor, JwtProvidor>();
        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        
       services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.issuer,
                ValidAudience = jwtSettings.audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });
        return services;
    }
}
