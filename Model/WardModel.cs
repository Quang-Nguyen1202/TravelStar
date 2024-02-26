using TravelStar.Entities;

namespace TravelStar.Model;
public class WardModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int DistrictId { get; set; }

    public District? District { get; set; }
    public ICollection<Hotel>? Hotels { get; set; }
}
