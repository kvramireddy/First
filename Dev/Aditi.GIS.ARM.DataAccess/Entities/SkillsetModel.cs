using System;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.ARM.Shared;
using FluentNHibernate.Mapping;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class SkillsetModel : VersionableEntityModelBase, ISkillset
    {
        public virtual string ShortName { get; set; }
        public virtual string LongName { get; set; }
        public virtual string Description { get; set; }
    }

    public class SkillsetMapper : ClassMap<SkillsetModel>
    {
        public SkillsetMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.LongName, "Name");
            this.Map(x => x.IsActive, "IsActive");
            this.Table("skillset");
            this.Schema("gis");
        }
    }

    public class EmployeeSkillsetModel : MToMTableModel<EmployeeModel, SkillsetModel>
    {
        public virtual bool IsPrimary { get; set; }
    }

    public class EmployeeSkillsetMapper : ClassMap<EmployeeSkillsetModel>
    {
        public EmployeeSkillsetMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.IsPrimary, "IsPrimary");
            this.Map(x => x.IsActive, "IsActive");
            this.References(x => x.LeftItem, "empid");
            this.References(x => x.RightItem, "skillsetid");
            this.Table("empskillset");
            this.Schema("gis");
        }
    }
}
