using System.Text.Json.Serialization;
using api.Enums;

namespace api.DTOs.Salons;

public class SalonGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))] //converts enum to string and back
    public City City { get; set; }
    public string Address { get; set; } = null!;
    public string? Image { get; set; }
}