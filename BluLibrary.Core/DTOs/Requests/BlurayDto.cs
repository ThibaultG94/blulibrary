using System.ComponentModel.DataAnnotations;
using BluLibrary.Core.Entities;

namespace BluLibrary.Core.DTOs.Requests;

// DTO pour la création d'un nouveau Blu-ray
public class CreateBlurayDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Director { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?:\d{13}|\d{10})$", ErrorMessage = "ISBN must be 10 or 13 digits")]
    public string ISBN { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int ReleaseYear { get; set; }

    [Required]
    public BlurayGenre Genre { get; set; }

    [Range(1, 1000)]
    public int DurationMinutes { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Studio { get; set; }
}

// DTO pour la mise à jour d'un Blu-ray existant
public class UpdateBlurayDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Director { get; set; } = string.Empty;

    [Required]
    public BlurayGenre Genre { get; set; }

    [Range(1, 1000)]
    public int DurationMinutes { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Studio { get; set; }
}

// DTO pour la réponse, contenant toutes les informations d'un Blu-ray
public class BlurayResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? Description { get; set; }
    public BlurayGenre Genre { get; set; }
    public int DurationMinutes { get; set; }
    public string? Studio { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? LastModified { get; set; }
}