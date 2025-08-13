using IELTSMockApi.Data;
using IELTSMockApi.DTOs;
using IELTSMockApi.Models;
using IELTSMockApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IELTSMockApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var exists = await _context.Users.AnyAsync(u =>
            u.Username.ToLower() == dto.Username.ToLower() ||
            u.Email.ToLower() == dto.Email.ToLower());

        if (exists)
            return BadRequest("Username or email already exists");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password // NOTE: store hashed in production
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);

        if (user == null)
            return Unauthorized("Invalid username or password");

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            Token = token
        });
    }
}
