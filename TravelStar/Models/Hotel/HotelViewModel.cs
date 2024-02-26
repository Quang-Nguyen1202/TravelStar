using TravelStar.Model;

namespace TravelStar.Site.Models.Hotel;
public class HotelViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Hotline { get; set; }
    public int Star { get; set; }
    public int WardId { get; set; }
    public string? Ward { get; set; }
    public string? District { get; set; }
    public string? City { get; set; }
    public int ImageId { get; set; }
    public long MinPrice { get; set; }
    public string? EmailOwner { get; set; }
    public bool IsSuperAdmin { get; set; }
}