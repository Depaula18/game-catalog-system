using FluentValidation;
using GameCatalogSystem.Application.DTOs;
using GameCatalogSystem.Application.DTOs.Game;
using GameCatalogSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IValidator<CreateGameRequestDTO> _createValidator;
    private readonly IValidator<UpdateGameRequestDTO> _updateValidator;
    private readonly IWebHostEnvironment _environment;
    public GamesController(
            IGameService gameService,
            IValidator<CreateGameRequestDTO> createValidator,
            IValidator<UpdateGameRequestDTO> updateValidator,
            IWebHostEnvironment environment) 
    {
        _gameService = gameService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _environment = environment; 
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDTO<GameResponseDTO>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null)
    {
        var pagedResult = await _gameService.GetAllPaginatedAsync(page, pageSize, search);
        return Ok(pagedResult);
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
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(new { Errors = errors });
        }

        var game = await _gameService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _gameService.DeleteAsync(id);
        if (!result) return NotFound("Jogo não encontrado para exclusão.");

        return NoContent(); 
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GameResponseDTO>> Update(Guid id, [FromBody] UpdateGameRequestDTO dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(new { Errors = errors });
        }

        var game = await _gameService.UpdateAsync(id, dto);
        if (game == null) return NotFound("Jogo não encontrado.");

        return Ok(game);
    }

    [HttpPost("{id:guid}/cover")]
    public async Task<IActionResult> UploadCover(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".webp")
            return BadRequest("Formato inválido. Envie apenas imagens (JPG, PNG, WEBP).");

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var coverUrl = $"/uploads/{uniqueFileName}";

        var result = await _gameService.UpdateCoverUrlAsync(id, coverUrl);
        if (!result) return NotFound("Jogo não encontrado.");

        return Ok(new { CoverUrl = coverUrl });
    }

}
