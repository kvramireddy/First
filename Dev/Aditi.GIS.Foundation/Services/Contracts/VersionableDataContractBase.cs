using System;
using Aditi.GIS.Foundation.Shared;
using Newtonsoft.Json;

namespace Aditi.GIS.Foundation.Services.Contracts
{
    public abstract class VersionableDataContractBase : DataContractBase, IContractBase, IDto
    {
        [JsonIgnore]
        public DateTimeOffset StartDate { get; set; }
        [JsonIgnore]
        public DateTimeOffset? EndDate { get; set; }

        public virtual void Transfer(IDto source)
        {
            base.Transfer(source);
            VersionableDataContractBase vdata = source as VersionableDataContractBase;
            if (vdata != null)
            {
                this.StartDate = vdata.StartDate;
                this.EndDate = vdata.EndDate;
            }
        }
    }
}
