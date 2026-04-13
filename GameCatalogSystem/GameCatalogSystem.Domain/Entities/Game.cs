using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Domain.Entities
{
    public class Game : BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public decimal Price { get; private set; }

        public Guid GenreId { get; private set; }

        public Genre? Genre { get; private set; }

        protected Game() { }

        public Game(string title, string description, decimal price, DateTime releaseDate, Guid genreId)
        {
            Title = title;
            Description = description;
            Price = price;
            ReleaseDate = releaseDate;
            GenreId = genreId;
        }

        public void Update(string title, string description, decimal price, DateTime releaseDate, Guid genreId)
        {
            Title = title;
            Description = description;
            Price = price;
            ReleaseDate = releaseDate;
            GenreId = genreId;
        }


    }
}
