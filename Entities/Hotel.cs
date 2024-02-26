namespace TravelStar.Entities;
public class Hotel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Hotline { get; set; }
    public int Star { get; set; }
    public int WardId { get; set; }
    public int ImageId { get; set; }
    public string? UserId { get; set; }

    public Ward? Ward { get; set; }
    public AppUser? User { get; set; }
    public ICollection<Room>? Rooms { get; set; }
}