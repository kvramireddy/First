using System;
using System.Linq;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.ARM.Shared;
using FluentNHibernate.Mapping;
using System.Collections.Generic;
using Aditi.GIS.Foundation;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class LocationModel : EntityModelBase, ILocation
    {
        public virtual string CountryId { get; set; }
        public virtual string CountryName { get; set; }

        public virtual string ShortName { get; set; }
        public virtual string LongName { get; set; }
        public virtual string Description { get; set; }

        public virtual GeoCoordinate Coordinate
        {
            get
            {
                return new GeoCoordinate(this.Latitude, this.Longitude);
            }

            set
            {
                this.Longitude = value.Lon;
                this.Latitude = value.Lat;
            }
        }

        public virtual float Longitude { get; set; }
        public virtual float Latitude { get; set; }

        // public virtual IEnumerable<EmployeeModel> Employees
        // {
        //     get
        //     {
        //         return this.EmployeeLocations == null ? null : this.EmployeeLocations.Select(x => x.LeftItem);
        //     }
        // }
        // 
        // public virtual IList<JunctionTableModel<EmployeeModel, LocationModel>> EmployeeLocations { get; set; }
    }

    public class LocationMapper : ClassMap<LocationModel>
    {
        public LocationMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.LongName, "Name");
            this.Map(x => x.Latitude, "Latitude");
            this.Map(x => x.Longitude, "Longitude");
            this.Map(x => x.IsActive, "IsActive");
            // this.HasMany(x => x.EmployeeLocations).LazyLoad();
            this.Table("location");
            this.Schema("gis");
        }
    }

    public class EmployeeLocationMapper : ClassMap<MToMTableModel<EmployeeModel, LocationModel>>
    {
        public EmployeeLocationMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.IsActive, "IsActive");
            this.References(x => x.LeftItem, "empid");
            this.References(x => x.RightItem, "locid");
            this.Table("emplocation");
            this.Schema("gis");
        }
    }
}
