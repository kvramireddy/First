using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.ARM.Shared
{
    public interface IValueType : IIdentityObject
    {
        // long Id { get; set; }
        string ShortName { get; set; }
        string LongName { get; set; }
        string Description { get; set; }
    }
}
