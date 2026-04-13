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
    public class GenreRepository : IGenreRepository
    {
        private readonly CatalogDbContext _context;

        public GenreRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(Guid id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
        }

        public void Update(Genre genre)
        {
            _context.Genres.Update(genre);
        }

        public void Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }

