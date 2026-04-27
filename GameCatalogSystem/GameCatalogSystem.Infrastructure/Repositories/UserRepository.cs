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

public class UserRepository : IUserRepository
{
    private readonly CatalogDbContext _context;

    public UserRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
