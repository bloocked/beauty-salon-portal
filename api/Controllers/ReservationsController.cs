using api.Data;
using api.DTOs.Reservations;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ReservationsController(ApiDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Reservation>> PostReservation(ReservationCreateDto reservationDto)
    {
        var service = await _context.SpecialistServices
        .FirstOrDefaultAsync(s =>
        s.SpecialistId == reservationDto.SpecialistId &&
        s.Id == reservationDto.SpecialistServiceId);

        if (service == null) return NotFound("Service doesnt exist");

        if (service.Duration.TotalMinutes < 15) 
            return BadRequest("Service duration incorrect, must be longer than 15 min");

        var newEndTime = reservationDto.StartTime.Add(service.Duration);

        bool overlap = await _context.Reservations
            .AnyAsync(r => r.SpecialistId == reservationDto.SpecialistId &&
            r.StartTime < newEndTime &&
            r.EndTime > reservationDto.StartTime);

        if (overlap) return BadRequest("Reservations cannot overlap");

        var reservation = new Reservation
        {
            SpecialistServiceId = reservationDto.SpecialistServiceId,
            SpecialistId = reservationDto.SpecialistId,
            ClientId = reservationDto.ClientId,
            StartTime = reservationDto.StartTime,
            EndTime = newEndTime
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        //return CreatedAtAction() eventually with reservation, for now just
        return Created("", reservationDto);
    }
}