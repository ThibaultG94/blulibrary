using System.Net;
using System.Net.Http.Json;
using BluLibrary.Core.DTOs.Requests;
using BluLibrary.Core.Entities;
using BluLibrary.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BluLibrary.Tests.IntegrationTests.Api;

// Cette classe contient tous nos tests d'intégration pour le contrôleur Bluray
public class BlurayControllerTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly BluLibraryContext _context;

    public BlurayControllerTests(WebApplicationFactory<Program> factory)
    {
        // On configure une factory personnalisée qui utilise une base de données en mémoire
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // On remplace le DbContext normal par un qui utilise une base en mémoire
                var descriptor = services.SingleOrDefault(d => 
                    d.ServiceType == typeof(DbContextOptions<BluLibraryContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<BluLibraryContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });

        _scope = _factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<BluLibraryContext>();
        _client = _factory.CreateClient();
    }

    // Cette méthode s'exécute avant chaque test
    public async Task InitializeAsync()
    {
        // On s'assure que la base est vide avant chaque test
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        
        // On peut ajouter des données de test communes ici
        await SeedTestData();
    }

    // Cette méthode s'exécute après chaque test
    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllBlurays()
    {
        // Arrange est déjà fait dans InitializeAsync

        // Act - On fait un appel HTTP GET à notre API
        var response = await _client.GetAsync("/api/v1/bluray");
        var blurays = await response.Content.ReadFromJsonAsync<IEnumerable<BlurayResponseDto>>();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(blurays);
        Assert.NotEmpty(blurays);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturnCreatedResponse()
    {
        // Arrange
        var newBluray = new CreateBlurayDto
        {
            Title = "Inception",
            Director = "Christopher Nolan",
            ISBN = "1234567890",
            ReleaseYear = 2010,
            Genre = BlurayGenre.SciFi,
            DurationMinutes = 148
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/bluray", newBluray);
        var createdBluray = await response.Content.ReadFromJsonAsync<BlurayResponseDto>();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdBluray);
        Assert.Equal(newBluray.Title, createdBluray.Title);
        
        // Vérifie que l'URL de la ressource créée est correct
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"/api/v1/bluray/{createdBluray.Id}", 
            response.Headers.Location?.ToString() ?? string.Empty);
    }

    [Fact]
    public async Task Update_WithValidData_ShouldReturnNoContent()
    {
        // Arrange - On crée d'abord un Bluray
        var createDto = new CreateBlurayDto
        {
            Title = "Original Title",
            Director = "Original Director",
            ISBN = "1234567890",
            ReleaseYear = 2010,
            Genre = BlurayGenre.Action,
            DurationMinutes = 120
        };
        var createResponse = await _client.PostAsJsonAsync("/api/v1/bluray", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<BlurayResponseDto>();
        
        var updateDto = new UpdateBlurayDto
        {
            Title = "Updated Title",
            Director = "Updated Director",
            Genre = BlurayGenre.Drama,
            DurationMinutes = 130
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/bluray/{created?.Id}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Vérifie que les modifications ont bien été appliquées
        var getResponse = await _client.GetAsync($"/api/v1/bluray/{created?.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<BlurayResponseDto>();
        
        Assert.NotNull(updated);
        Assert.Equal(updateDto.Title, updated.Title);
        Assert.Equal(updateDto.Director, updated.Director);
    }

    private async Task SeedTestData()
    {
        // Ajoute quelques données de test
        var bluray = Bluray.Create(
            "The Matrix",
            "Lana Wachowski",
            "0987654321",
            1999,
            BlurayGenre.SciFi,
            136
        );

        await _context.Blurays.AddAsync(bluray);
        await _context.SaveChangesAsync();
    }
}