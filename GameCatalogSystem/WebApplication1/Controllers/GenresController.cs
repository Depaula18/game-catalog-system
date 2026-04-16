using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameCatalogSystem.Application.DTOs.Genre;
using GameCatalogSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;
    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreResponseDTO>>> GetAll()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GenreResponseDTO>> GetById(Guid id)
    {
        var genre = await _genreService.GetByIdAsync(id);
        if (genre == null) return NotFound();
        return Ok(genre);
    }

    [HttpPost]
    public async Task<ActionResult<GenreResponseDTO>> Create([FromBody] CreateGenreRequestDTO dto)
    {
        var genre = await _genreService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
    }

}

