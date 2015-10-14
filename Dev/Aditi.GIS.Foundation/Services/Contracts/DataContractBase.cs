using System.Collections.Generic;
using System.Runtime.Serialization;
using Aditi.GIS.Foundation.Shared;
using Newtonsoft.Json;

namespace Aditi.GIS.Foundation.Services.Contracts
{
    public abstract class DataContractBase : IContractBase
    {
        // [JsonProperty(PropertyName = "id")]
        // [DataMember(Name = "id")]
        // public long Id { get; set; }
        // [JsonProperty(PropertyName = "etag")]
        // [DataMember(Name = "etag")]
        [JsonIgnore]
        public string ETag { get; set; }

        [JsonProperty(PropertyName = "extprops")]
        [DataMember(Name = "extprops")]
        public Dictionary<string, string> ExtendedProperties { get; set; }

        [JsonProperty(PropertyName = "links")]
        [DataMember(Name = "links")]
        public Dictionary<string, string> Links { get; set; }

        public virtual void SetDefaultLinks()
        {
        }

        public virtual void SetSelfLink()
        {
        }

        public virtual void Transfer(IDto source)
        {
            IIdentityObject data = source as IIdentityObject;
            if (data != null)
            {
                // this.Id = data.Id;
                // this.ETag = data.ETag;
            }
        }
    }
}
