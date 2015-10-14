using System.Collections.Generic;
using System.Runtime.Serialization;
using Aditi.GIS.ARM.Shared;
using Aditi.GIS.Foundation;
using Aditi.GIS.Foundation.Services.Contracts;
using Aditi.GIS.Foundation.Shared;
using Newtonsoft.Json;

namespace Aditi.GIS.ARM.Contracts
{
    public sealed class Location : DataContractBase, ILocation
    {
        public override void SetDefaultLinks()
        {
            if (this.Links == null)
            {
                this.Links = new Dictionary<string, string>();
            }

            this.SetSelfLink();
            this.Links["employee"] = "location/" + this.Id + "/employee";
        }

        public override void SetSelfLink()
        {
            if (this.Links == null)
            {
                this.Links = new Dictionary<string, string>();
            }

            this.Links["self"] = "location/" + this.Id;
        }

        [JsonProperty(PropertyName = "id")]
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [JsonIgnore]
        public string CountryId { get; set; }
        [JsonIgnore]
        public string CountryName { get; set; }

        [JsonIgnore]
        public string ShortName { get; set; }

        [JsonProperty(PropertyName = "ln")]
        [DataMember(Name = "ln")]
        public string LongName { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        [DataMember(Name = "pos")]
        [JsonProperty(PropertyName = "pos")]
        public GeoCoordinate Coordinate { get; set; }
        // [DataMember(Name = "lon")]
        // [JsonProperty(PropertyName = "lon")]
        // public long Longitude { get; set; }
        // 
        // [DataMember(Name = "lat")]
        // [JsonProperty(PropertyName = "lat")]
        // public long Latitude { get; set; }

        public override void Transfer(IDto source)
        {
            base.Transfer(source);
            ILocation data = source as ILocation;
            if (data != null)
            {
                this.Id = data.Id;
                this.CountryId = data.CountryId;
                this.CountryName = data.CountryName;
                this.ShortName = data.ShortName;

                this.LongName = data.LongName;
                this.Description = data.Description;

                this.Coordinate = data.Coordinate;
                this.SetSelfLink();
            }
        }
    }
}
