using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Specialist
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public int SalonId { get; set; }

    // navigation properties
    public User User { get; set; } = null!;
    public Salon Salon { get; set; } = null!;
    public ICollection<SpecialistService> Services { get; } = new List<SpecialistService>();
}