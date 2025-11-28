using api.Enums;
using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class User : IdentityUser<int>
{
    // identity now handles Id, UserName, Email, PasswordHash
    
    //optional navigation properties
    public Specialist? Specialist { get; set; }
    public ICollection<Reservation> Reservations = new List<Reservation>();
} 