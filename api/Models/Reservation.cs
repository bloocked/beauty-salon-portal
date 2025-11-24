namespace api.Models;

public class Reservation
{
    public int Id { get; set; }
    public int SpecialistId { get; set; }
    public int UserId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartTime { get; set; }

    // navigation properties

    public Specialist Specialist { get; set; } = null!;
    public User Client { get; set; } = null!;
}