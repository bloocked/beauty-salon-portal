using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Reservations;

public class ReservationCreateDto
{
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int SpecialistId { get; set; }
    [Required]
    public int SpecialistServiceId { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
}