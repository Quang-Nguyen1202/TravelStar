using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelStar.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public int Beds { get; set; }
        public bool HasBreakfast { get; set; }
        public int HotelId { get; set; }
        public int ImageId { get; set; }
        public long Price { get; set; }

        public Hotel? Hotel { get; set; }
    }
}
