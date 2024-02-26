using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelStar.Entities
{
    public class Ward
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int DistrictId { get; set; }

        public District? District { get; set; }
        public ICollection<Hotel>? Hotels { get; set; }
    }
}
