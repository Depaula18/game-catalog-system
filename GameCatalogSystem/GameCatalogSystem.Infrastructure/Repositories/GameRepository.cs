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

    public async Task<(IEnumerable<Game> Games, int TotalCount)> GetAllPaginatedAsync(int page, int pageSize, string? searchTerm)
    {
        var query = _context.Games.Include(g => g.Genre).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(g => g.Title.Contains(searchTerm) || g.Description.Contains(searchTerm));
        }

        int totalCount = await query.CountAsync();

        var games = await query
            .OrderBy(g => g.Title) 
            .Skip((page - 1) * pageSize) 
            .Take(pageSize) 
            .ToListAsync(); 

        return (games, totalCount);
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

