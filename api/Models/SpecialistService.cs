namespace api.Models;

public class SpecialistService
{
    public int Fk_SpecialistId { get; set; }
    public int Fk_ServiceId { get; set; }
    public required double Cost { get; set; }
    public required TimeSpan Duration { get; set; }

    // navigation properties
    public required Service Service { get; set;}
    public required Specialist Specialist { get; set; }
}