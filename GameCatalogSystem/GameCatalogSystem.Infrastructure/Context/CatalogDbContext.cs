using GameCatalogSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Infrastructure.Context;

    public class CatalogDbContext : DbContext
    {

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) {}

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasKey(g => g.Id);
            modelBuilder.Entity<Genre>().Property(g => g.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Genre>().Property(g => g.Description).HasMaxLength(500);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Game>().HasKey(g => g.Id);
            modelBuilder.Entity<Game>().Property(g => g.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Game>().Property(g => g.Price).HasPrecision(18, 2);

            modelBuilder.Entity<Game>().HasOne(g => g.Genre).WithMany().HasForeignKey(g => g.GenreId).OnDelete(DeleteBehavior.Restrict);
    }

    }

