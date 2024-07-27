using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullStackAPI.Data;
using FullStackAPI.Models;
using NLog;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AuthController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User model)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == model.Username);

        if (user == null || !VerifyPassword(user.Password, model.Password))
        {
            // Check if the user is locked out
            if (user != null && user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
            {
                var timeLeft = (user.LockoutEnd.Value - DateTime.Now).TotalSeconds;
                return BadRequest(new { message = $"Please wait {Math.Ceiling(timeLeft)} seconds before trying again." });
            }

            if (user != null)
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 3)
                {
                    user.LockoutEnd = DateTime.Now.AddSeconds(15); // Lock for 15 seconds
                }
                await _context.SaveChangesAsync();
            }
            return Ok(new { message = "test successful" });
            //return BadRequest(new { message = "Invalid username or password" });
        }

        // Reset failed login attempts and lockout
        user.FailedLoginAttempts = 0;
        user.LockoutEnd = null; // Ensure LockoutEnd is set to null
        await _context.SaveChangesAsync();

        return Ok(new { message = "Login successful" });
    }

    private bool VerifyPassword(string storedPassword, string enteredPassword)
    {
        // Implement your password verification logic here
        return storedPassword == enteredPassword;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        Logger.Info("GetAllUsers endpoint called");
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }
}
