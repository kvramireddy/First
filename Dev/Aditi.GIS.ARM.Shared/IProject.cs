using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.ARM.Shared
{
    public interface IProject : IValueType, IDto, IVersionable, IIdentityObject
    {
        DateTimeOffset ProjectStartDate { get; set; }
        DateTimeOffset? ProjectEndDate { get; set; }
    }
}
