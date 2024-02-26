using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class BookingBo : BaseBo<BookingModel, Booking>, IBookingBo
{
    public BookingBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}
