using GameCatalogSystem.Application.DTOs;
using GameCatalogSystem.Application.DTOs.Game;
using GameCatalogSystem.Application.Services.Interfaces;
using GameCatalogSystem.Domain.Entities;
using GameCatalogSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GameCatalogSystem.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IAuditService _auditService;

    public GameService(IGameRepository gameRepository, IGenreRepository genreRepository, IAuditService auditService)
    {
        _gameRepository = gameRepository;
        _genreRepository = genreRepository;
        _auditService = auditService; // NOVO
    }

    public async Task<PagedResponseDTO<GameResponseDTO>> GetAllPaginatedAsync(int page, int pageSize, string? searchTerm)
    {
        var (games, totalCount) = await _gameRepository.GetAllPaginatedAsync(page, pageSize, searchTerm);

        var gamesDto = games.Select(g => new GameResponseDTO
        {
            Id = g.Id,
            Title = g.Title,
            Description = g.Description,
            Price = g.Price,
            ReleaseDate = g.ReleaseDate,
            GenreName = g.Genre != null ? g.Genre.Name : "Gênero não encontrado",
            CoverUrl = g.CoverUrl
        });

        return new PagedResponseDTO<GameResponseDTO>(gamesDto, totalCount, page, pageSize);
    }

    public async Task<GameResponseDTO?> GetByIdAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null) return null;

        return new GameResponseDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate,
            GenreName = game.Genre != null ? game.Genre.Name : "Gênero não encontrado"
        };
    }

    public async Task<GameResponseDTO> CreateAsync(CreateGameRequestDTO dto)
    {
        var genre = await _genreRepository.GetByIdAsync(dto.GenreId);
        if (genre == null)
        {
            throw new Exception("Gênero inválido. Não é possível cadastrar o jogo.");
        }

        var dataLancamentoUtc = DateTime.SpecifyKind(dto.ReleaseDate, DateTimeKind.Utc);

        var game = new Game(dto.Title, dto.Description, dto.Price, dataLancamentoUtc, dto.GenreId);

        await _gameRepository.AddAsync(game);
        await _gameRepository.SaveChangesAsync();

        var detailsJson = JsonSerializer.Serialize(dto);
        await _auditService.LogAsync("Create", "Game", game.Id.ToString(), detailsJson);

        return new GameResponseDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate, 
            GenreName = genre.Name
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null) return false;

        _gameRepository.Delete(game);
        await _gameRepository.SaveChangesAsync();
        return true;
    }

    public async Task<GameResponseDTO?> UpdateAsync(Guid id, UpdateGameRequestDTO dto)
    {

        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null) return null;


        var genre = await _genreRepository.GetByIdAsync(dto.GenreId);
        if (genre == null) throw new Exception("Gênero inválido.");


        game.Update(dto.Title, dto.Description, dto.Price, dto.ReleaseDate, dto.GenreId);


        _gameRepository.Update(game);
        await _gameRepository.SaveChangesAsync();

        return new GameResponseDTO
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate,
            GenreName = genre.Name
        };
    }

    public async Task<bool> UpdateCoverUrlAsync(Guid id, string coverUrl)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null) return false;

        game.UpdateCoverUrl(coverUrl);

        _gameRepository.Update(game);
        await _gameRepository.SaveChangesAsync();

        return true;
    }
}
