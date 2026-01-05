using api.Data;
using api.Models;
using api.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.DTOs.Specialists;
using api.DTOs.SpecialistServices;
using System.Runtime.Intrinsics.X86;
using SQLitePCL;


namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialistsController : ControllerBase
{
    private readonly ApiDbContext _context;
    public SpecialistsController(ApiDbContext context)
    {
        _context = context;
    }

    // TODO, Look into the randomness that is the get all endpoint using service name and get singular using service id

    // GET: api/specialists
    [HttpGet]
    public async Task<ActionResult<List<Specialist>>> GetSpecialists(
        [FromQuery] int? salonId,
        [FromQuery] string? serviceName)
    {
        var query = _context.Specialists
        .Include(s => s.User)
        .Include(s => s.SpecialistServices)
        .ThenInclude(ss => ss.Service)
        .AsQueryable();

        if (salonId != null)
        {
            query = query.Where(s => s.SalonId == salonId);
        }

        if (!string.IsNullOrEmpty(serviceName))
        {
            query = query.Where(specialist =>
                specialist.SpecialistServices.Any(sService =>
                    sService.Service.Name == serviceName));
        }

        var specialists = await query.ToListAsync();

        //cleaned it up
        var specialistDtos = specialists.Select(s => new SpecialistGetDto 
        {
            Id = s.Id,
            Name = s.User.Username,
            Email = s.User.Email,
            Services = s.SpecialistServices
            .Where(ss => string.IsNullOrEmpty(serviceName) || ss.Service.Name == serviceName)
            .Select(ss => new SpecialistServiceGetDto
            {
                Id = ss.Id,
                Name = ss.Service.Name,
                Cost = ss.Cost,
                Duration = ss.Duration

            }).ToList()
        }).ToList();

        if (specialistDtos.Count == 0) return NotFound("Specialists list is empty");

        return Ok(specialistDtos);
    }

    // GET: api/specialists/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecialist(int id)
    {
        var result = await _context.Specialists
        .Include(s => s.User)
        .Include(s => s.SpecialistServices)
        .ThenInclude(ss => ss.Service)
        .Include(s => s.Salon)
        .FirstOrDefaultAsync(s => s.Id == id);

        if (result == null) return NotFound();

        var responseUser = new SpecialistGetDto
        {
            Id = result.Id,
            Name = result.User.Username,
            Email = result.User.Email,
            Services = result.SpecialistServices.Select(ss => new SpecialistServiceGetDto
            {
                Id = ss.Id,
                Name = ss.Service.Name,
                Cost = ss.Cost,
                Duration = ss.Duration
            }).ToList()
        };

        return Ok(responseUser);
    }

    [HttpGet("{specialistId}/occupied-slots")]
    public async Task<IActionResult> GetOccupiedSlots(
        [FromRoute] int specialistId,
        [FromQuery] DateTime date)
    {
        TimeSpan interval = TimeSpan.FromMinutes(15);

        var selectedDate = date.Date;

        var reservations = await _context.Reservations
        .Where(r => r.SpecialistId == specialistId && r.StartTime.Date == selectedDate)
        .Include(r => r.SpecialistService)
        .ToListAsync();

        var occupiedIntervals = reservations.Select(r => new OccupiedSlotDto
        {
            StartTime = r.StartTime,
            EndTime = r.EndTime
        })
        .OrderBy(r => r.StartTime)
        .ToList();

        return Ok(occupiedIntervals);
    }
}