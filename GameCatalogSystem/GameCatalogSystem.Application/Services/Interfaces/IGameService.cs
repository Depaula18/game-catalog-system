using GameCatalogSystem.Application.DTOs;
using GameCatalogSystem.Application.DTOs.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Application.Services.Interfaces;

public interface IGameService
{
    Task<PagedResponseDTO<GameResponseDTO>> GetAllPaginatedAsync(int page, int pageSize, string? searchTerm);
    Task<GameResponseDTO?> GetByIdAsync(Guid id);
    Task<GameResponseDTO> CreateAsync(CreateGameRequestDTO dto);
    Task<bool> DeleteAsync(Guid id);
    Task<GameResponseDTO?> UpdateAsync(Guid id, UpdateGameRequestDTO dto);
}
