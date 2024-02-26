namespace TravelStar.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public long TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public bool IsConfirm { get; set; }
        public string? ConfirmCode { get; set; }
        public DateTime? PaymentDate { get; set; }

        public Room? Room { get; set; }
        public Customer? Customer { get; set; }
    }
}
