﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Aditi.GIS.ARM.Shared;
using Aditi.GIS.Foundation.Services.Contracts;
using Aditi.GIS.Foundation.Shared;
using Newtonsoft.Json;

namespace Aditi.GIS.ARM.Contracts
{
    public sealed class Level : VersionableDataContractBase, ILevel
    {
        public override void SetDefaultLinks()
        {
            if (this.Links == null)
            {
                this.Links = new Dictionary<string, string>();
            }

            this.SetSelfLink();
            this.Links["employee"] = "level/" + this.Id + "/employee";
        }

        public override void SetSelfLink()
        {
            if (this.Links == null)
            {
                this.Links = new Dictionary<string, string>();
            }

            this.Links["self"] = "level/" + this.Id;
        }

        [JsonProperty(PropertyName = "id")]
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [JsonIgnore]
        public string ShortName { get; set; }

        [JsonProperty(PropertyName = "ln")]
        [DataMember(Name = "ln")]
        public string LongName { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        public override void Transfer(IDto source)
        {
            base.Transfer(source);
            ILevel data = source as ILevel;
            if (data != null)
            {
                this.Id = data.Id;
                this.ShortName = data.ShortName;
                this.LongName = data.LongName;
                this.Description = data.Description;
                this.SetSelfLink();
            }
        }
    }
}
