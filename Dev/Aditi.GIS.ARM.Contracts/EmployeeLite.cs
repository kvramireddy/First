using System.Collections.Generic;
using System.Runtime.Serialization;
using Aditi.GIS.ARM.Shared;
using Aditi.GIS.Foundation.Services.Contracts;
using Aditi.GIS.Foundation.Shared;
using Newtonsoft.Json;

namespace Aditi.GIS.ARM.Contracts
{
    public class EmployeeLite : DataContractBase
    {
        public override void SetDefaultLinks()
        {
            if (this.Links == null)
            {
                this.Links = new Dictionary<string, string>();
            }

            this.SetSelfLink();
            this.Links["location"] = "employee/" + this.EmpId + "/location";
            this.Links["skillset"] = "employee/" + this.EmpId + "/skillset";
            this.Links["project"] = "employee/" + this.EmpId + "/project";
            this.Links["stream"] = "employee/" + this.EmpId + "/stream";
            this.Links["level"] = "employee/" + this.EmpId + "/level";
        }

        public override void SetSelfLink()
        {
            if (this.Links == null)
            {
                this.Links = new Dictionary<string, string>();
            }

            this.Links["self"] = "employee/" + this.EmpId;
        }

        [JsonIgnore]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "id")]
        [DataMember(Name = "id")]
        public string EmpId { get; set; }

        [JsonProperty(PropertyName = "fn")]
        [DataMember(Name = "fn")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "ln")]
        [DataMember(Name = "ln")]
        public string LastName { get; set; }

        public override void Transfer(IDto source)
        {
            IEmployee data = source as IEmployee;
            if (data != null)
            {
                this.EmpId = data.EmpId;
                this.FirstName = data.FirstName;
                this.LastName = data.LastName;

                this.SetSelfLink();
            }
        }

    }
}
