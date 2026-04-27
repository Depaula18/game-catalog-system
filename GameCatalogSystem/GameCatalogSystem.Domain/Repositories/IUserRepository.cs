using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Domain.Entities;

namespace GameCatalogSystem.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    void Add(User user);
    Task SaveChangesAsync();
}
