using System.Text.RegularExpressions;
using BluLibrary.Core.Entities.Base;
using BluLibrary.Core.Exceptions;

namespace BluLibrary.Core.Entities;

public sealed class Bluray : BaseEntity
{
    public const int MaxTitleLength = 200;
    public const int MaxDirectorLength = 100;
    private static readonly Regex IsbnRegex = new(@"^(?:\d{13}|\d{10})$", RegexOption.Compiled);
    public string Title { get; private set; }
    public string Director { get; private set; }
    public string ISBN { get; private set; }
    public int ReleaseYear { get; private set; }
    public string? Description { get; private set; }
    public BlurayGenre Genre { get; private set; }
    public int DurationMinutes { get; private set; }
    public string? Studio { get; private set; }
    public DateTime DateAdded { get; private set; }
    public DateTime? DateModified { get; private set; }

    private Bluray(
        string title,
        string director,
        string isbn,
        int releaseYear,
        BlurayGenre genre,
        int durationMinutes)
    {
        SetTitle(title);
        SetDirector(director);
        SetISBN(isbn);
        SetReleaseYear(releaseYear);
        SetGenre(genre);
        SetDuration(durationMinutes);
        DateAdded = DateTime.UtcNow;
    }

    public static Bluray Create(
        string title,
        string director,
        string isbn,
        int releaseYear,
        BlurayGenre genre,
        int durationMinutes)
    {
        return new Bluray(title, director, isbn, releaseYear, genre, durationMinutes);
    }

    public void Update(
        string title,
        string director,
        BlurayGenre genre,
        int durationMinutes,
        string? description = null,
        string? studio = null)
    {
        SetTitle(title);
        SetDirector(director);
        SetGenre(genre);
        SetDuration(durationMinutes);
        Description = description;
        Studio = studio;
        LastModified = DateTime.UtcNow;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("Title cannot be empty");
        if (title.Length > MaxTitleLength)
            throw new DomainValidationException($"Title cannot exceed {MaxTitleLength} characters");

        Title = title.Trim();
    }

    private void SetDirector(string director)
    {
        if (string.IsNullOrWhiteSpace(director))
            throw new DomainValidationException("Director cannot be empty");
        if (director.Length > MaxDirectorLength)
            throw new DomainValidationException($"Director cannot exceed {MaxDirectorLength} characters");

        Director = director.Trim();
    }

    private void SetISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new DomainValidationException("ISBN cannot be empty");
        if (!IsbnRegex.IsMatch(isbn))
            throw new DomainValidationException("ISBN must be either 10 or 13 digits");

        ISBN = isbn;
    }

    private void SetReleaseYear(int releaseYear)
    {
        if (releaseYear < 1900 || releaseYear > DateTime.UtcNow.Year)
            throw new DomainValidationException($"Release year must be between 1900 and {DateTime.UtcNow.Year}");

        ReleaseYear = releaseYear;
    }

    private void SetGenre(BlurayGenre genre)
    {
        if (!Enum.IsDefined(genre))
            throw new DomainValidationException("Invalid genre");

        Genre = genre;
    }

    private void SetDuration(int durationMinutes)
    {
        if (durationMinutes <= 0 || durationMinutes > 1000)
            throw new DomainValidationException("Duration must be between 1 and 1000 minutes");

        DurationMinutes = durationMinutes;
    }
}

public enum BlurayGenre
{
    Action,
    Adventure,
    Animation,
    Biography,
    Comedy,
    Crime,
    Drama,
    Documentary,
    Family,
    Fantasy,
    FilmNoir,
    History,
    Horror,
    Music,
    Musical,
    Mystery,
    Romance,
    SciFi,
    Sport,
    Thriller,
    War,
    Western
}