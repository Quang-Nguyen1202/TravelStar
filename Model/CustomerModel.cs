using TravelStar.Entities;

namespace TravelStar.Model;
public class CustomerModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDay { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}
