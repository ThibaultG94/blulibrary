public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        // ... autres middlewares
        return app;
    }
}