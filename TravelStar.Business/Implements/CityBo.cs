using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class CityBo : BaseBo<CityModel, City>, ICityBo
{
    public CityBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}
