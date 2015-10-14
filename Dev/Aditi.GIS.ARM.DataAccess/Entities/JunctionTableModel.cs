using Aditi.GIS.Foundation.DataAccess.Entities;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class MToMTableModel<TLeft, TRight> : EntityModelBase
        where TLeft : EntityModelBase
        where TRight : EntityModelBase
    {
        public virtual TLeft LeftItem { get; set; }
        public virtual TRight RightItem { get; set; }
    }

    public class MToMVersionableTableModel<TLeft, TRight> : VersionableEntityModelBase
        where TLeft : EntityModelBase
        where TRight : EntityModelBase
    {
        public virtual TLeft LeftItem { get; set; }
        public virtual TRight RightItem { get; set; }
    }
}
