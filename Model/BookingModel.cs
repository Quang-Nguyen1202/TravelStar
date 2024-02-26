using TravelStar.Entities;

namespace TravelStar.Model;
public class BookingModel
{
    public int Id { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime? CancelDate { get; set; }
    public long TotalPrice { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public int CustomerId { get; set; }
    public int RoomId { get; set; }
    public bool IsConfirm { get; set; }
    public string? ConfirmCode { get; set; }

    public Room? Room { get; set; }
    public Customer? Customer { get; set; }
}
