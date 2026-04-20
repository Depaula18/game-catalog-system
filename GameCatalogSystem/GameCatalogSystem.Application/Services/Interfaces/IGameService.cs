using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Application.DTOs.Game;

namespace GameCatalogSystem.Application.Services.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameResponseDTO>> GetAllAsync();
    Task<GameResponseDTO?> GetByIdAsync(Guid id);
    Task<GameResponseDTO> CreateAsync(CreateGameRequestDTO dto);
    Task<bool> DeleteAsync(Guid id);
}
