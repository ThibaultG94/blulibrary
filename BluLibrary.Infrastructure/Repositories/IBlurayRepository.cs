using BluLibrary.Core.Entities;

namespace BluLibrary.Core.Interfaces.Repositories;

public interface IBlurayRepository
{
    Task<Bluray?> GetByIdAsync(Guid id);
    Task<Bluray?> GetByIsbnAsync(string isbn);
    Task<IEnumerable<Bluray>> GetAllAsync();
    Task<IEnumerable<Bluray>> SearchAsync(string searchTerm);
    Task AddAsync(Bluray bluray);
    Task UpdateAsync(Bluray bluray);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> IsbnExistsAsync(string isbn);
}