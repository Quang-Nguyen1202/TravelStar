﻿using TravelStar.Entities;

namespace TravelStar.Model;
public class DistrictModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CityId { get; set; }

    public City? City { get; set; }
    public ICollection<Ward>? Wards { get; set; }
}
