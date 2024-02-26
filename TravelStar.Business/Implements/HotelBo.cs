using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class HotelBo : BaseBo<HotelModel, Hotel>, IHotelBo
{
    public HotelBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}