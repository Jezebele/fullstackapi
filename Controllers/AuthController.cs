using Microsoft.Extensions.Options;
using NLog;
using FullStackAPI.Data;
using FullStackAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly JwtTokenService _jwtTokenService;  // JwtTokenService'yi ekleyin
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AuthController(DataContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User model)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == model.Username);

        if (user == null || !VerifyPassword(user.Password, model.Password))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = _jwtTokenService.GenerateToken(user);  // Token oluşturmayı buradan yapın

        return Ok(new { message = "Login successful", token });
    }

    private bool VerifyPassword(string storedPassword, string enteredPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        Logger.Info("GetAllUsers endpoint called");
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User model)
    {
        if (await _context.Users.AnyAsync(u => u.Username == model.Username))
        {
            return BadRequest(new { message = "Username already exists" });
        }

        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        // Hash the password before saving
        model.Password = HashPassword(model.Password);

        _context.Users.Add(model);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registration successful" });
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}