namespace api.Models;

public class Service
{
    public int Id { get; set; }
    public required string Name { get; set; }

    //navigation properties
    public SpecialistService SpecialistService { get; set; } = null!;
}