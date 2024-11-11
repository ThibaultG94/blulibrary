public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration des services API
        services.AddControllers();
        services.AddSwagger();
        // ... autres configurations
        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Configuration Swagger
        return services;
    }
}