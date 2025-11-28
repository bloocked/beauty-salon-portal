using api.Data;
using api.Models;
using api.DTOs.Users;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApiDbContext _context;
    public UsersController(ApiDbContext context)
    {
        _context = context;
    }

    //currently not using dtos since its mostly for testing
    //if there is a need to keep, will sanitize
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        if (users.Count == 0) return NotFound("Users list is empty");

        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _context.Users.FindAsync(id);

        if (result == null) return NotFound();

        var responseUser = new UserGetDto
        {
            Id = result.Id,
            Username = result.Username,
            Email = result.Email
        };

        return Ok(responseUser);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserCreateDto userDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
        {
            return BadRequest("Email already exists");
        }

        if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
        {
            return BadRequest("Username is taken");
        }

        var User = new User
        {
            Username = userDto.Username,
            PasswordHash = Hasher.Hash(userDto.Password),
            Email = userDto.Email
        };

        _context.Users.Add(User);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
        nameof(GetUser),
        new { id = User.Id },
        new UserGetDto
        {
            Id = User.Id,
            Username = User.Username,
            Email = User.Email
        });
    }
}