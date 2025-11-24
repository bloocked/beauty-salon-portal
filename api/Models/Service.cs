namespace api.Models;

public class Service
{
    public int Id { get; set; }
    public required string Name { get; set; }

    //navigation properties
    public ICollection<SpecialistService> SpecialistServices { get; } = new List<SpecialistService>();
}