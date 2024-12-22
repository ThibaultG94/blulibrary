public class IsbnTests
{
    [Theory]
    [InlineData("0-7475-3269-9")]  // ISBN-10 avec tirets
    [InlineData("0 7475 3269 9")]  // ISBN-10 avec espaces
    [InlineData("0747532699")]     // ISBN-10 sans séparateurs
    [InlineData("978-0-7475-3269-9")]  // ISBN-13 avec tirets
    [InlineData("9780747532699")]      // ISBN-13 sans séparateurs
    public void Create_WithValidISBN_ShouldSucceed(string validIsbn)
    {
        // Act
        var isbn = ISBN.Create(validIsbn);

        // Assert
        Assert.NotNull(isbn);
        Assert.Matches(@"^\d{10}|\d{13}$", isbn.Value);
    }

    [Theory]
    [InlineData("0-7475-3269-X")]  // ISBN-10 avec X
    public void Create_WithValidISBN10WithX_ShouldSucceed(string validIsbn)
    {
        // Act
        var isbn = ISBN.Create(validIsbn);

        // Assert
        Assert.NotNull(isbn);
        Assert.Matches(@"^\d{9}X$", isbn.Value);
    }

    [Theory]
    [InlineData("0-7475-3269-0")]  // Checksum invalide
    [InlineData("978-0-7475-3269-0")]  // Checksum invalide
    [InlineData("abc")]  // Trop court
    [InlineData("12345678901234")]  // Trop long
    [InlineData("abcdefghij")]  // Lettres invalides
    public void Create_WithInvalidISBN_ShouldThrowException(string invalidIsbn)
    {
        // Act & Assert
        Assert.Throws<DomainValidationException>(() => ISBN.Create(invalidIsbn));
    }
}