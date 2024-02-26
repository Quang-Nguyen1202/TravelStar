namespace TravelStar.Site.Models.Room;
public class CreateRoomModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Type { get; set; }
    public int Beds { get; set; }
    public bool HasBreakfast { get; set; }
    public int HotelId { get; set; }
    public long Price { get; set; }

    public List<IFormFile> Images { get; set; } = new List<IFormFile>();
}
