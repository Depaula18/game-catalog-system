using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Application.DTOs.Genre;

namespace GameCatalogSystem.Application.Services.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreResponseDTO>> GetAllAsync();
    Task<GenreResponseDTO?> GetByIdAsync(Guid id);
    Task<GenreResponseDTO> CreateAsync(CreateGenreRequestDTO request);
}

