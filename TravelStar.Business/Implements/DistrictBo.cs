using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
 public class DistrictBo : BaseBo<DistrictModel, District>, IDistrictBo
{
    public DistrictBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}
