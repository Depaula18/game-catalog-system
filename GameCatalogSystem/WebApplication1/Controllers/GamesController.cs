using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameCatalogSystem.Application.DTOs.Game;
using GameCatalogSystem.Application.Services.Interfaces;

namespace GameCatalogSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponseDTO>>> GetAll()
    {
        var games = await _gameService.GetAllAsync();
        return Ok(games);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GameResponseDTO>> GetById(Guid id)
    {
        var game = await _gameService.GetByIdAsync(id);
        if (game == null) return NotFound("Jogo não encontrado.");
        return Ok(game);
    }

    [HttpPost]
    public async Task<ActionResult<GameResponseDTO>> Create([FromBody] CreateGameRequestDTO dto)
    {
        try
        {
            var game = await _gameService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _gameService.DeleteAsync(id);
        if (!result) return NotFound("Jogo não encontrado para exclusão.");

        return NoContent(); // Retornamos 204 (Sucesso, mas sem conteúdo no corpo)
    }
}
