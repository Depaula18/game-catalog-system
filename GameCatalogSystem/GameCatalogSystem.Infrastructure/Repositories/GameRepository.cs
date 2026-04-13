using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Domain.Entities;
using GameCatalogSystem.Domain.Repositories;
using GameCatalogSystem.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GameCatalogSystem.Infrastructure.Repositories;
public class GameRepository : IGameRepository
{
    private readonly CatalogDbContext _context;

    public GameRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _context.Games.Include(g => g.Genre).ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _context.Games.Include(g => g.Genre).FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
    }

    public void Update(Game game)
    {
        _context.Games.Update(game);
    }

    public void Delete(Game game)
    {
        _context.Games.Remove(game);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

