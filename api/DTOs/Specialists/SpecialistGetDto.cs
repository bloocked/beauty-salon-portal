using api.DTOs.SpecialistServices;

namespace api.DTOs.Specialists;

public class SpecialistGetDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? SalonName { get; set; }
    public List<SpecialistServiceGetDto> Services { get; set; } = new ();

}