using System.Text.RegularExpressions;

namespace BluLibrary.Core.ValueObjects;

public sealed class ISBN
{
    private static readonly Regex CleanupRegex = new(@"[\s-]", RegexOptions.Compiled);
    private readonly string _value;

    public string Value => _value;

    private ISBN(string value)
    {
        _value = value;
    }

    public static ISBN Create(string input)
    {
        // Nettoyer l'entrée
        var cleaned = CleanupRegex.Replace(input, "");
        
        if (string.IsNullOrWhiteSpace(cleaned))
            throw new DomainValidationException("ISBN ne peut pas être vide");

        // Vérifier la longueur
        if (cleaned.Length != 10 && cleaned.Length != 13)
            throw new DomainValidationException("ISBN doit avoir 10 ou 13 chiffres");

        // Vérifier que ce sont uniquement des chiffres (et 'X' pour ISBN-10)
        if (!cleaned.All(c => char.IsDigit(c) || (cleaned.Length == 10 && c == 'X')))
            throw new DomainValidationException("ISBN ne peut contenir que des chiffres, et 'X' pour ISBN-10");

        // Valider le checksum
        bool isValid = cleaned.Length == 10 
            ? ValidateIsbn10(cleaned)
            : ValidateIsbn13(cleaned);

        if (!isValid)
            throw new DomainValidationException("ISBN invalide : checksum incorrect");

        return new ISBN(cleaned);
    }

    private static bool ValidateIsbn10(string isbn)
    {
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (10 - i) * (isbn[i] - '0');
        }

        var lastChar = isbn[9];
        if (lastChar == 'X')
            sum += 10;
        else
            sum += (isbn[9] - '0');

        return sum % 11 == 0;
    }

    private static bool ValidateIsbn13(string isbn)
    {
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            sum += (i % 2 == 0 ? 1 : 3) * (isbn[i] - '0');
        }

        var checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit == (isbn[12] - '0');
    }

    public static implicit operator string(ISBN isbn) => isbn._value;
    
    public override string ToString() => _value;
}