using GameCatalogSystem.Application.DTOs.Auth;
using GameCatalogSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request.Name, request.Email, request.Password);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }
}