using api.Data;
using api.Models;
using api.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    public UsersController(UserManager<User> context)
    {
        _userManager = context;
    }

    //currently not using dtos since its mostly for testing
    //if there is a need to keep, will sanitize
    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _userManager.Users.Select(u => new UserGetDto
        {
            Id = u.Id,
            Username = u.UserName!,
            Email = u.Email
        })
        .ToListAsync();

        if (users.Count == 0) return NotFound("Users list is empty");

        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userManager.Users
            .Where(u => u.Id == id)
            .Select(u => new UserGetDto
            {
                Id = u.Id,
                Username = u.UserName!,
                Email = u.Email
            })
            .FirstAsync();

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
        if (await _userManager.Users.AnyAsync(u => u.Email == userDto.Email))
        {
            return BadRequest("Email already exists");
        }

        if (await _userManager.Users.AnyAsync(u => u.UserName == userDto.Username))
        {
            return BadRequest("Username is taken");
        }

        var user = new User
        {
            UserName = userDto.Username,
            Email = userDto.Email
        };

        var result = await _userManager.CreateAsync(user, userDto.Password); //creates and adds

        if(!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, "User"); // NEED TO ACTUALLY CREATE ROLES

        return CreatedAtAction(
        nameof(GetUser),
        new { id = user.Id },
        new UserGetDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email
        });
    }
}