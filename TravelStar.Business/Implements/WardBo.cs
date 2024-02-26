using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class WardBo : BaseBo<WardModel, Ward>, IWardBo
{
    public WardBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}
