using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Domain.Entities;

namespace GameCatalogSystem.Domain.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(Guid id);
    Task AddAsync(Genre genre);
    void Update(Genre genre);
    void Delete(Genre genre);
    Task SaveChangesAsync(); // O Unit of Work simplificado
}
