namespace api.DTOs.SpecialistServices;

public class SpecialistServiceGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double? Cost { get; set; }
    public TimeSpan? Duration { get; set; }
}