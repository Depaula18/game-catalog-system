using GameCatalogSystem.Domain.Entities;

namespace GameCatalogSystem.Domain.Repositories;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(Guid id);
    Task AddAsync(Game game);
    void Update(Game game);
    void Delete(Game game);
    Task SaveChangesAsync();
}