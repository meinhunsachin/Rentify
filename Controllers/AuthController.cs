using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (_context.Users.Any(u => u.Email == user.Email))
        {
            return BadRequest("User already exists.");
        }

        // Hash the password (implement a method to hash passwords)
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        // Generate JWT token (implement a method to generate tokens)
        var token = GenerateJwtToken(user);

        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        // Implement token generation logic here
        return "generated-jwt-token";
    }
}
