namespace SurveyBasket;

public  static class DependancyInjection
{

    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {

        services.AddControllers();
        services.AddOpenApiServices()
                .AddFluentValidationService()
                .AddRegisteredServices();


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
