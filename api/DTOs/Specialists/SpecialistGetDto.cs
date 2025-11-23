namespace api.DTOs.Specialists;

public class SpecialistGetDto
{
    public int UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string SalonName { get; set; }

}