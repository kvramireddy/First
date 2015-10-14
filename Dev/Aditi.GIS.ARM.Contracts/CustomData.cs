using System.Collections.Generic;
using Aditi.GIS.Foundation.Services.Contracts;

namespace Aditi.GIS.ARM.Contracts
{
    public class CustomData : DataContractBase
    {
        public CustomData()
        {
            this.ExtendedProperties = new Dictionary<string, string>();
        }
    }
}
