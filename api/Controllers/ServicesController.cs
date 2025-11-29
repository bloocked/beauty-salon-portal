using api.Data;
using api.DTOs.Services;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ServicesController(ApiDbContext context)
    {
        _context = context;
    }

    // GET: api/services
    //[Authorize(Roles = "Specialist")]
    [HttpGet]
    public async Task<ActionResult<List<Service>>> GetServices()
    {
        var services = await _context.Services.ToListAsync();

        if (services.Count == 0) return NotFound("Services list is empty");

        return Ok(services);
    }

    // GET: api/services/{id}
    //[Authorize(Roles = "Specialist")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetService(int id)
    {
        var result = await _context.Services.FindAsync(id);

        if (result == null) return NotFound();

        var responseUser = new ServiceGetDto
        {
            Id = result.Id,
            Name = result.Name
        };

        return Ok(responseUser);
    }
}