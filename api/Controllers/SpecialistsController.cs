using api.Data;
using api.Models;
using api.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.DTOs.Specialists;


namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialistsController : ControllerBase
{
    private readonly ApiContext _context;
    public SpecialistsController(ApiContext context)
    {
        _context = context;
    }

    // GET: api/specialists
    [HttpGet]
    public async Task<ActionResult<List<Specialist>>> GetSpecialists()
    {
        var Specs = await _context.Specialists.ToListAsync();

        if (Specs.Count == 0) return NotFound("Specialists list is empty");

        return Ok(Specs);
    }

    // GET: api/specialists/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecialist(int id)
    {
        var result = await _context.Specialists
        .Include(s => s.User)
        .Include(s => s.Salon)
        .FirstOrDefaultAsync(s => s.UserId == id);

        if (result == null) return NotFound();

        var responseUser = new SpecialistGetDto
        {
            UserId = result.UserId,
            Name = result.User.UserName!,
            Email = result.User.Email!,
            SalonName = result.Salon.Name
        };

        return Ok(responseUser);
    }

    // // POST: api/users
    // [HttpPost]
    // public async Task<ActionResult<User>> PostUser(UserCreateDto userDto)
    // {
    //     if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
    //     {
    //         return BadRequest("Email already exists");
    //     }

    //     if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
    //     {
    //         return BadRequest("Username is taken");
    //     }

    //     var User = new User
    //     {
    //         Username = userDto.Username,
    //         Password = userDto.Password,
    //         Email = userDto.Email
    //     };

    //     _context.Users.Add(User);
    //     await _context.SaveChangesAsync();

    //     return CreatedAtAction(
    //     nameof(GetUser),
    //     new { id = User.Id },
    //     new UserGetDto
    //     {
    //         Id = User.Id,
    //         Username = User.Username,
    //         Email = User.Email
    //     });
    // }
}