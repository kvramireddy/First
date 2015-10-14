using System;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.DataAccess.Entities
{
    public abstract class VersionableEntityModelBase : EntityModelBase, IEntityModelBase, IDto
    {
        public virtual DateTimeOffset StartDate { get; set; }
        public virtual DateTimeOffset? EndDate { get; set; }

        public virtual void Transfer(IDto source)
        {
            base.Transfer(source);
            VersionableEntityModelBase vdata = source as VersionableEntityModelBase;
            if (vdata != null)
            {
                this.StartDate = vdata.StartDate;
                this.EndDate = vdata.EndDate;
            }
        }
    }
}
