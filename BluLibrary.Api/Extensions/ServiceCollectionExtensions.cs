using BluLibrary.Core.Interfaces.Repositories;
using BluLibrary.Infrastructure.Data.Context;
using BluLibrary.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BluLibrary.Api.Extensions;

public static class ServiceCollectionExtensions
{
    // Méthode principale qui orchestre toutes nos configurations de services
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration de la base de données
        services.AddDbContext<BluLibraryContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                // Configuration spécifique pour SqlServer
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3); // Retry 3 fois en cas d'échec
                    sqlOptions.CommandTimeout(30); // Timeout de 30 secondes
                }
            ));

        // Enregistrement des repositories
        services.AddScoped<IBlurayRepository, BlurayRepository>();

        // Configuration des contrôleurs API
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Configuration JSON personnalisée si nécessaire
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        // Configuration de la documentation API
        services.AddEndpointsApiExplorer();
        services.AddSwagger();

        // Configuration CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Pour un front-end React par exemple
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
        });

        return services;
    }

    // Configuration détaillée de Swagger
    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BluLibrary API",
                Version = "v1",
                Description = "API de gestion de bibliothèque de Blu-ray",
                Contact = new OpenApiContact
                {
                    Name = "Équipe BluLibrary",
                    Email = "contact@blulibrary.com"
                }
            });

            // Ajout de la documentation XML si configurée
            var xmlFile = "BluLibrary.Api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}