using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.ARM.Shared
{
    public interface IVersionable : IDto
    {
        DateTimeOffset StartDate { get; set; }
        DateTimeOffset? EndDate { get; set; }
    }
}
