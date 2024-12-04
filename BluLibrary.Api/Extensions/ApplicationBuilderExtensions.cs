namespace BluLibrary.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    // Configuration du pipeline HTTP
    public static IApplicationBuilder UseApiConfiguration(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        // En développement, on active Swagger
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BluLibrary API V1");
                c.RoutePrefix = string.Empty; // Pour avoir Swagger à la racine
            });
        }

        // Middleware de sécurité et configuration standard
        app.UseHttpsRedirection();
        app.UseRouting();
        
        // Configuration CORS
        app.UseCors("AllowSpecificOrigins");

        // Middleware d'authentification et d'autorisation
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            // Configuration de health checks si nécessaire
            endpoints.MapHealthChecks("/health");
        });

        return app;
    }
}