using BluLibrary.Core.Entities;
using BluLibrary.Core.Exceptions;

namespace BluLibrary.Tests.UnitTests.Core.Entities;

public class BlurayTests
{
    // Tests de création
    [Fact]
    public void Create_WithValidData_ShouldCreateBluray()
    {
        // Arrange - On prépare les données de test
        var title = "The Matrix";
        var director = "Lana Wachowski";
        var isbn = "1234567890";
        var releaseYear = 1999;
        var genre = BlurayGenre.SciFi;
        var duration = 136;

        // Act - On exécute l'action à tester
        var bluray = Bluray.Create(title, director, isbn, releaseYear, genre, duration);

        // Assert - On vérifie les résultats
        Assert.NotNull(bluray);
        Assert.Equal(title, bluray.Title);
        Assert.Equal(director, bluray.Director);
        Assert.Equal(isbn, bluray.ISBN);
        Assert.Equal(releaseYear, bluray.ReleaseYear);
        Assert.Equal(genre, bluray.Genre);
        Assert.Equal(duration, bluray.DurationMinutes);
        Assert.NotEqual(Guid.Empty, bluray.Id);
        Assert.NotEqual(default, bluray.DateAdded);
    }

    // Tests des validations
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidTitle_ShouldThrowException(string invalidTitle)
    {
        // Arrange
        var director = "Christopher Nolan";
        var isbn = "1234567890";
        var releaseYear = 2010;
        var genre = BlurayGenre.Action;
        var duration = 150;

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() =>
            Bluray.Create(invalidTitle, director, isbn, releaseYear, genre, duration));
        
        Assert.Equal("Title cannot be empty", exception.Message);
    }

    [Theory]
    [InlineData("12345")]        // Trop court
    [InlineData("123456789A")]   // Contient une lettre
    [InlineData("12345678901234")] // Trop long
    public void Create_WithInvalidIsbn_ShouldThrowException(string invalidIsbn)
    {
        // Arrange
        var title = "Inception";
        var director = "Christopher Nolan";
        var releaseYear = 2010;
        var genre = BlurayGenre.Action;
        var duration = 150;

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() =>
            Bluray.Create(title, director, invalidIsbn, releaseYear, genre, duration));
        
        Assert.Equal("ISBN must be either 10 or 13 digits", exception.Message);
    }

    [Fact]
    public void Update_ShouldModifyBlurayAndSetLastModified()
    {
        // Arrange
        var bluray = Bluray.Create(
            "Initial Title",
            "Initial Director",
            "1234567890",
            2020,
            BlurayGenre.Action,
            120
        );

        var newTitle = "Updated Title";
        var newDirector = "Updated Director";
        var newGenre = BlurayGenre.Drama;
        var newDuration = 130;
        var newDescription = "New description";
        var newStudio = "New Studio";

        // On s'assure qu'un peu de temps s'est écoulé
        Thread.Sleep(1);

        // Act
        bluray.Update(newTitle, newDirector, newGenre, newDuration, newDescription, newStudio);

        // Assert
        Assert.Equal(newTitle, bluray.Title);
        Assert.Equal(newDirector, bluray.Director);
        Assert.Equal(newGenre, bluray.Genre);
        Assert.Equal(newDuration, bluray.DurationMinutes);
        Assert.Equal(newDescription, bluray.Description);
        Assert.Equal(newStudio, bluray.Studio);
        Assert.NotNull(bluray.LastModified);
        Assert.True(bluray.LastModified > bluray.DateAdded);
    }

    [Theory]
    [InlineData(0)]      // Trop court
    [InlineData(-1)]     // Négatif
    [InlineData(1001)]   // Trop long
    public void Create_WithInvalidDuration_ShouldThrowException(int invalidDuration)
    {
        // Arrange
        var title = "The Dark Knight";
        var director = "Christopher Nolan";
        var isbn = "1234567890";
        var releaseYear = 2008;
        var genre = BlurayGenre.Action;

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() =>
            Bluray.Create(title, director, isbn, releaseYear, genre, invalidDuration));
        
        Assert.Equal("Duration must be between 1 and 1000 minutes", exception.Message);
    }

    [Theory]
    [InlineData(1899)]   // Trop ancien
    [InlineData(2525)]   // Dans le futur
    public void Create_WithInvalidReleaseYear_ShouldThrowException(int invalidYear)
    {
        // Arrange
        var title = "Test Movie";
        var director = "Test Director";
        var isbn = "1234567890";
        var genre = BlurayGenre.Action;
        var duration = 120;

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() =>
            Bluray.Create(title, director, isbn, invalidYear, genre, duration));
        
        Assert.StartsWith("Release year must be between 1900 and", exception.Message);
    }
}