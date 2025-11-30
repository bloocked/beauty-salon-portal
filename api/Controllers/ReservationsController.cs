using api.Data;
using api.DTOs.Reservations;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ReservationsController(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<Reservation>> PostReservation(ReservationCreateDto reservationDto)
    {
        var service = await _context.SpecialistServices
        .FirstOrDefaultAsync(s => s.Id == reservationDto.SpecialistServiceId);

        if (service == null) return NotFound("Service doesnt exist");

        var newEndTime = reservationDto.StartTime.Add(service.Duration);

        bool overlap = await _context.Reservations
            .AnyAsync(r => r.SpecialistId == reservationDto.SpecialistId &&
            r.StartTime < newEndTime &&
            r.StartTime.Add(service.Duration) > reservationDto.StartTime);

        if (overlap) return BadRequest("Reservations cannot overlap");

        var reservation = new Reservation
        {
            SpecialistServiceId = reservationDto.SpecialistServiceId,
            SpecialistId = reservationDto.SpecialistId,
            StartTime = reservationDto.StartTime
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        //return CreatedAtAction() eventually, for now just
        return Created("", reservationDto);
    }
}