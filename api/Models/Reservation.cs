namespace api.Models;

public class Reservation
{
    public int Id { get; set; }
    public int SpecialistServiceId { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; }

    // navigation properties

    public SpecialistService SpecialistService { get; set; } = null!;
    public User Client { get; set; } = null!;
}