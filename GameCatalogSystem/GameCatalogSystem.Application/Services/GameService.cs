using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Application.DTOs.Game;
using GameCatalogSystem.Application.Services.Interfaces;
using GameCatalogSystem.Domain.Entities;
using GameCatalogSystem.Domain.Repositories;

namespace GameCatalogSystem.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGenreRepository _genreRepository;

    public GameService(IGameRepository gameRepository, IGenreRepository genreRepository)
    {
        _gameRepository = gameRepository;
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GameResponseDTO>> GetAllAsync()
    {
        var games = await _gameRepository.GetAllAsync();
        return games.Select(g => new GameResponseDTO
        {
            Id = g.Id,
            Title = g.Title,
            Description = g.Description,
            Price = g.Price,
            ReleaseDate = g.ReleaseDate,
            GenreName = g.Genre != null ? g.Genre.Name : "Gênero não encontrado"
        });
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
        if(genre == null)
        {
            throw new Exception("Gênero inválido. Não é possível cadastrar o jogo.");
        }

        var game = new Game(dto.Title, dto.Description, dto.Price, dto.ReleaseDate, dto.GenreId);

        await _gameRepository.AddAsync(game);
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

    public async Task<bool> DeleteAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null) return false;

        _gameRepository.Delete(game);
        await _gameRepository.SaveChangesAsync();
        return true;
    }
}
