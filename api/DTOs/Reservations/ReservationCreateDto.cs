namespace api.DTOs.Reservations;

public class ReservationCreateDto
{
    public int SpecialistServiceId { get; set; }
    public int SpecialistId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartTime { get; set; }
}