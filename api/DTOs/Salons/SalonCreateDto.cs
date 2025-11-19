using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using api.Enums;

namespace api.DTOs.Salons;

public class SalonCreateDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))] //converts enum to string and back
    public City City { get; set; }
    [Required]
    public string Address { get; set; } = null!;
    public string? Image { get; set; }
}