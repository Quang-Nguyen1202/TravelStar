using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class RoomBo : BaseBo<RoomModel, Room>, IRoomBo
{
    public RoomBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}