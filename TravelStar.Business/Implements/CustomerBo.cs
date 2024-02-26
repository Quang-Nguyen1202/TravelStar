using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;

namespace TravelStar.Business.Implements;
public class CustomerBo : BaseBo<CustomerModel, Customer>, ICustomerBo
{
    public CustomerBo(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}
