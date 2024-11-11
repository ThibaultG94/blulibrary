using BluLibrary.Core.Entities;
using BluLibrary.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BluLibrary.Infrastructure.Data.Context;

public class BluLibraryContext : DbContext
{
    public DbSet<Bluray> Blurays { get; set; } = null!;

    public BluLibraryContext(DbContextOptions<BluLibraryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlurayConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}