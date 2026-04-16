using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Application.DTOs.Genre;
using GameCatalogSystem.Application.Services.Interfaces;
using GameCatalogSystem.Domain.Entities;
using GameCatalogSystem.Domain.Repositories;

namespace GameCatalogSystem.Application.Services;
public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    public async Task<IEnumerable<GenreResponseDTO>> GetAllAsync()
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres.Select(g => new GenreResponseDTO
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        });
    }

    public async Task<GenreResponseDTO?> GetByIdAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null) return null;
        return new GenreResponseDTO
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };
    }

    public async Task<GenreResponseDTO> CreateAsync(CreateGenreRequestDTO dto)
    {
        var genre = new Genre(dto.Name, dto.Description);

        await _genreRepository.AddAsync(genre);
        await _genreRepository.SaveChangesAsync();

        return new GenreResponseDTO
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };
    }
}
