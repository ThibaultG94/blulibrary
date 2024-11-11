using BluLibrary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BluLibrary.Infrastructure.Data.Configurations;

public class BlurayConfiguration : IEntityTypeConfiguration<Bluray>
{
    public void Configure(EntityTypeBuilder<Bluray> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(Bluray.MaxTitleLength);

        builder.Property(b => b.Director)
            .IsRequired()
            .HasMaxLength(Bluray.MaxDirectorLength);

        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(13);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.Studio)
            .HasMaxLength(100);

        builder.Property(b => b.DateAdded)
            .IsRequired();

        // Index sur ISBN car c'est souvent utilisé pour les recherches
        builder.HasIndex(b => b.ISBN).IsUnique();
        
        // Index sur Title pour améliorer les performances des recherches
        builder.HasIndex(b => b.Title);
    }
}