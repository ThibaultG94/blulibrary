using BluLibrary.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// La méthode AddApiServices encapsule maintenant la configuration détaillée des services
builder.Services.AddApiServices(builder.Configuration);

// Configuration additionelle des contrôleurs et des options API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construction de l'application
var app = builder.Build();

// Configure the HTTP request pipeline.
// En développement, on active Swagger et les outils de développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else 
{
    // En production, on utilise un gestionnaire d'erreurs personnalisé
    app.UseExceptionHandler("/Error");
    // Le défaut HSTS est 30 jours. Vous pouvez le modifier pour les scénarios de production.
    app.UseHsts();
}

// Middleware de sécurité
app.UseHttpsRedirection();

// Configuration CORS si nécessaire
app.UseCors("AllowSpecificOrigins");

// Middleware d'authentification et d'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Configuration du routage
app.UseRouting();

// Endpoints de l'API
app.MapControllers();

// Point de terminaison de surveillance de la santé de l'application
app.MapHealthChecks("/health");

// Configuration des routes et des endpoints par défaut si nécessaire
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // Vous pouvez ajouter d'autres configurations d'endpoints ici
});

// Démarrage de l'application
app.Run();

// Si vous utilisez des tests d'intégration, vous pouvez avoir besoin d'exposer l'application
// Décommentez la ligne suivante si nécessaire
// public partial class Program { }