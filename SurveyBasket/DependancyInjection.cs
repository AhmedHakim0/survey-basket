

namespace SurveyBasket;

public  static class DependancyInjection
{

    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApiServices()
                .AddFluentValidationService()
                .AddRegisteredServices();


        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;

    }

    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        return services;
    }

    public static IServiceCollection AddFluentValidationService(this IServiceCollection services)
    {

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;

    }


    public static IServiceCollection AddRegisteredServices(this IServiceCollection services)
    {

        services.AddScoped<IPollService, PollService>();

        return services;

    }

}
