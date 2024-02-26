using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelStar.Entities;

namespace TravelStar.Model;
public class HotelModel
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
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }

    public List<string> ImageUrls { get; set; } = new List<string>();
    public ICollection<RoomModel> Rooms { get; set; } = new List<RoomModel>();
}
