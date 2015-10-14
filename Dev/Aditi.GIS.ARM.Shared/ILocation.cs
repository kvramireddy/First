using System;
using Aditi.GIS.Foundation;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.ARM.Shared
{
    public interface ILocation : IValueType, IDto
    {
        string CountryId { get; set; }
        string CountryName { get; set; }

        GeoCoordinate Coordinate { get; set; }
    }
}
