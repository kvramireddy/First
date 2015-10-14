using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.DataAccess.Entities
{
    public abstract class EntityModelBase : IEntityModelBase, IDto
    {
        public virtual long Id { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual void Transfer(IDto source)
        {
            IEntityModelBase data = source as IEntityModelBase;
            if (data != null)
            {
                this.Id = data.Id;
                // this.ETag = data.ETag;
            }
        }
    }
}
