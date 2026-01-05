namespace api.DTOs.SpecialistServices;

public class SpecialistServiceGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal? Cost { get; set; }
    public TimeSpan? Duration { get; set; }
}