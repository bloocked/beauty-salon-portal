namespace api.Models;

public class Salon
{
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public string? Image { get; set; }

}