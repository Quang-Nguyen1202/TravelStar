using TravelStar.Entities;

namespace TravelStar.Model;
public class RoomModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Type { get; set; }
    public int Beds { get; set; }
    public bool HasBreakfast { get; set; }
    public int HotelId { get; set; }
    public long Price { get; set; }

    public List<string> ImageUrls { get; set; } = new List<string>();
    public HotelModel? Hotel { get; set; }
}
