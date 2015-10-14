using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.DataAccess.Entities
{
    public interface IEntityModelBase : IDto
    {
        long Id { get; set; }
        bool IsActive { get; set; }
    }
}
