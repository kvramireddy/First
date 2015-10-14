using System;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.ARM.Shared;
using FluentNHibernate.Mapping;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class LevelModel : VersionableEntityModelBase, ILevel
    {
        public virtual string ShortName { get; set; }
        public virtual string LongName { get; set; }
        public virtual string Description { get; set; }
    }

    public class LevelMapper : ClassMap<LevelModel>
    {
        public LevelMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.LongName, "Name");
            this.Map(x => x.IsActive, "IsActive");
            this.Table("levels");
            this.Schema("gis");
        }
    }

    public class EmployeeLevelMapper : ClassMap<MToMTableModel<EmployeeModel, LevelModel>>
    {
        public EmployeeLevelMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.IsActive, "IsActive");
            this.References(x => x.LeftItem, "empid");
            this.References(x => x.RightItem, "levelid");
            this.Table("emplevel");
            this.Schema("gis");
        }
    }
}
