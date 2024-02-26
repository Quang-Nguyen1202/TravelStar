namespace TravelStar.Site.Models.Revenue
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public string? CheckInDate { get; set; }
        public string? CheckOutDate { get; set; }
        public string? RoomName { get; set; }
        public string? CustomerEmail { get; set; }
        public long TotalPrice { get; set; }
        public string? HotelName { get; set; }
        public int HotelId { get; set; }
    }
}
