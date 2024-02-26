using AutoMapper;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Site.AutoMapper;
 public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CityModel, City>().ReverseMap();
        CreateMap<DistrictModel, District>().ReverseMap();
        CreateMap<WardModel, Ward>().ReverseMap();
        CreateMap<HotelModel, Hotel>().ReverseMap();
    }
}
