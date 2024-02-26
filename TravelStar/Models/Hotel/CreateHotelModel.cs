using TravelStar.Entities;

namespace TravelStar.Site.Models.Hotel
{
    public class CreateHotelModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Hotline { get; set; }
        public int Star { get; set; }
        public int WardId { get; set; }
        public IFormFile? Image { get; set; }
        public string? UserId { get; set; } 


        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
