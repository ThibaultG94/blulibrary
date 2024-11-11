using BluLibrary.Core.Entities;
using BluLibrary.Core.Interfaces.Repositories;
using BluLibrary.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BluLibrary.Infrastructure.Repositories;

public class BlurayRepository : IBlurayRepository
{
    private readonly BluLibraryContext _context;

    public BlurayRepository(BluLibraryContext context)
    {
        _context = context;
    }

    public async Task<Bluray?> GetByIdAsync(Guid id)
        => await _context.Blurays.FindAsync(id);

    public async Task<Bluray?> GetByIsbnAsync(string isbn)
        => await _context.Blurays.FirstOrDefaultAsync(b => b.ISBN == isbn);

    public async Task<IEnumerable<Bluray>> GetAllAsync()
        => await _context.Blurays.ToListAsync();

    public async Task<IEnumerable<Bluray>> SearchAsync(string searchTerm)
        => await _context.Blurays
            .Where(b => b.Title.Contains(searchTerm) || 
                       b.Director.Contains(searchTerm) ||
                       b.ISBN.Contains(searchTerm))
            .ToListAsync();

    public async Task AddAsync(Bluray bluray)
    {
        await _context.Blurays.AddAsync(bluray);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Bluray bluray)
    {
        _context.Blurays.Update(bluray);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var bluray = await GetByIdAsync(id);
        if (bluray != null)
        {
            _context.Blurays.Remove(bluray);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
        => await _context.Blurays.AnyAsync(b => b.Id == id);

    public async Task<bool> IsbnExistsAsync(string isbn)
        => await _context.Blurays.AnyAsync(b => b.ISBN == isbn);
}