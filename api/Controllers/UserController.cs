using api.Data;
using api.Models;
using api.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApiContext _context;
    public UsersController(ApiContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _context.Users.FindAsync(id);
        
        if(result == null) return NotFound();

        var responseUser = new ResponseUserDto
        {
            Id = result.Id,
            Username = result.Username,
            Email = result.Email
        };

        return Ok(responseUser);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(RegisterUserDto userDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
        {
            return BadRequest("Email already exists");
        }

        if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
        {
            return BadRequest("Username is taken");
        }
        
        var user = new User
        {
            Username = userDto.Username,
            Password = userDto.Password,
            Email = userDto.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser),
        new { id = user.Id },
        new { username = user.Username});
    }
}