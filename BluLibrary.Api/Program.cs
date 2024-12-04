using BluLibrary.Api.Extensions;
using BluLibrary.Api.Middleware.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

// Configuration des services
// ========================
// AddApiServices contient la configuration de base des services, y compris
// la base de données, les repositories, et les configurations de base
builder.Services.AddApiServices(builder.Configuration);

// Configuration des contrôleurs et des options API
// La validation automatique des modèles est activée par défaut
builder.Services.AddControllers();

// Configuration de la documentation API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construction de l'application
var app = builder.Build();

// Configuration du pipeline HTTP
// ============================

// Gestion globale des exceptions - DOIT être au début du pipeline
// pour capturer toutes les exceptions possibles
app.UseGlobalExceptionHandler();

// Configuration spécifique à l'environnement de développement
if (app.Environment.IsDevelopment())
{
    // Active Swagger uniquement en développement
    app.UseSwagger();
    app.UseSwaggerUI();
    // Permet d'avoir des messages d'erreur détaillés
    app.UseDeveloperExceptionPage();
}
else 
{
    // En production, on utilise des pages d'erreur personnalisées
    // et on force HTTPS pour la sécurité
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Sécurité et configuration de base
// ================================

// Force l'utilisation de HTTPS
app.UseHttpsRedirection();

// Configuration CORS pour permettre les appels depuis d'autres domaines
app.UseCors("AllowSpecificOrigins");

// Middleware d'authentification et d'autorisation
// À configurer selon vos besoins de sécurité
app.UseAuthentication();
app.UseAuthorization();

// Configuration du routage et des endpoints
// =======================================

// Nécessaire pour le routage des requêtes
app.UseRouting();

// Configuration des endpoints de l'API
app.MapControllers();

// Point de terminaison pour la surveillance de la santé de l'application
app.MapHealthChecks("/health");

// Configuration détaillée des endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    
    // Vous pouvez ajouter d'autres mappings d'endpoints ici
    // Par exemple, des endpoints pour SignalR ou gRPC
});

// Démarrage de l'application
app.Run();

// Cette ligne est nécessaire pour les tests d'intégration
// Elle permet d'accéder à la classe Program depuis les tests
public partial class Program { }