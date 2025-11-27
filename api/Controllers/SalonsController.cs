using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.DTOs.Salons;
using api.Enums;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalonsController : ControllerBase
{
    private readonly ApiContext _context;

    public SalonsController(ApiContext context)
    {
        _context = context;
    }

    // GET: api/salons
    [HttpGet]
    public async Task<ActionResult<List<Salon>>> GetSalons( //look more into query string usage
        [FromQuery] string? city,
        [FromQuery] string? service)
    {
        var query = _context.Salons.AsQueryable();

        if (!string.IsNullOrEmpty(city))
        {
            if (Enum.TryParse<City>(city, out var cityEnum)) // its late, read again
                query = query.Where(s => s.City == cityEnum);
        }

        if (!string.IsNullOrEmpty(service))
        {
            query = query.Where(salon =>  //might refactor in a bit
                salon.Specialists.Any(specialist =>
                    specialist.SpecialistServices.Any(sService =>
                        sService.Service.Name == service)
                    )
            );
        }

        var salons = await query.ToListAsync(); //use select() for dtos later

        if (salons.Count == 0) return NotFound("Salons list is empty");

        return Ok(salons);
    }

    // GET: api/salons/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalon(int id)
    {
        var result = await _context.Salons.FindAsync(id);

        if (result == null) return NotFound();

        var responseSalon = new SalonGetDto //could be redundant
        {
            Id = result.Id,
            Name = result.Name,
            City = result.City,
            Address = result.Address,
            Image = result.Image
        };

        return Ok(responseSalon);
    }

    // POST: api/salons
    [HttpPost]
    public async Task<ActionResult<SalonGetDto>> PostSalon(SalonCreateDto salonDto)
    {
        if (await _context.Salons.AnyAsync(s => s.Name == salonDto.Name))
        {
            return BadRequest("Salon name already exists");
        }

        if (await _context.Salons.AnyAsync(s => s.Address == salonDto.Address))
        {
            return BadRequest("Salon address already exists");
        }

        var Salon = new Salon
        {
            Name = salonDto.Name,
            City = salonDto.City,
            Address = salonDto.Address,
            Image = salonDto.Image
        };

        _context.Salons.Add(Salon);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetSalon),
            new { id = Salon.Id },
            new SalonGetDto
            {
                Id = Salon.Id,
                Name = Salon.Name,
                City = Salon.City,
                Address = Salon.Address,
                Image = Salon.Image
            });
    }
}