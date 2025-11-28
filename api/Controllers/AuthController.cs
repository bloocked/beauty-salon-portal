using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Data;
using api.DTOs.Auth;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly IConfiguration _jwtConfig;

    public AuthController(ApiDbContext context, IConfiguration configuration)
    {
        _context = context;
        _jwtConfig = configuration.GetSection("JWT");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Username == loginDto.Username);

        if (user == null) return Unauthorized("Username does not exist");

        if (!(Hasher.Hash(loginDto.Password) == user.PasswordHash))
            return Unauthorized("Incorrect password");

        //determine role based on set nav properties
        string role;
        if (user.Admin != null)
            role = "Admin";
        else if (user.Specialist != null)
            role = "Specialist";
        else
            role = "User";

        //jwt generation
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig["Secret"]!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _jwtConfig["ValidIssuer"],
            Audience = _jwtConfig["ValidAudience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new { token = jwt }); //anonymous obj with token property
    }
}