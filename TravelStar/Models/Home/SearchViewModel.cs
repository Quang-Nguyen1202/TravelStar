using TravelStar.Site.Models.Hotel;
namespace TravelStar.Site.Models.Home;
public class SearchViewModel
{
    public List<HotelViewModel> HotelList { get; set; } = new List<HotelViewModel>();
}
