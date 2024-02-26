using TravelStar.Entities;

namespace TravelStar.Model;
public class CityModel
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<District>? Districts { get; set; }
}
