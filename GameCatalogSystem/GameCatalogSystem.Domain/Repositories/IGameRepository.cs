using GameCatalogSystem.Domain.Entities;

namespace GameCatalogSystem.Domain.Repositories;

public interface IGameRepository
{
    Task<(IEnumerable<Game> Games, int TotalCount)> GetAllPaginatedAsync(int page, int pageSize, string? searchTerm);
    Task<Game?> GetByIdAsync(Guid id);
    Task AddAsync(Game game);
    void Update(Game game);
    void Delete(Game game);
    Task SaveChangesAsync();
}