namespace api.DTOs.Specialists;

public class SpecialistGetDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string SalonName { get; set; } = null!;

}