using Microsoft.AspNetCore.Identity;

namespace TravelStar.Entities;

public class AppUser : IdentityUser
{
    public string? UserFullName { get; set; }
    public string? Address { get; set; }
    public int Gender { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSupperAdmin { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<Hotel>? Hotels { get; set; }
}

