using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Domain.Entities;

public class User : BaseEntity
{ 
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }  

    protected User() { }

    public User(string name, string email, string passwordHash, string role = "Customer")
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
