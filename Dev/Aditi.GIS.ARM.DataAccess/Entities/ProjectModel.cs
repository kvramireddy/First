using System;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.ARM.Shared;
using FluentNHibernate.Mapping;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class ProjectModel : VersionableEntityModelBase, IProject
    {
        public virtual string ShortName { get; set; }
        public virtual string LongName { get; set; }
        public virtual string Description { get; set; }

        public virtual DateTimeOffset ProjectStartDate { get; set; }
        public virtual DateTimeOffset? ProjectEndDate { get; set; }
    }

    public class ProjectMapper : ClassMap<ProjectModel>
    {
        public ProjectMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.LongName, "Name");
            this.Map(x => x.IsActive, "IsActive");
            this.Table("project");
            this.Schema("gis");
        }
    }

    public class EmployeeProjectMapper : ClassMap<MToMVersionableTableModel<EmployeeModel, ProjectModel>>
    {
        public EmployeeProjectMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.IsActive, "IsActive");
            this.Map(x => x.StartDate, "startdate");
            this.Map(x => x.EndDate, "enddate");
            this.References(x => x.LeftItem, "empid");
            this.References(x => x.RightItem, "projectid");
            this.Table("empproject");
            this.Schema("gis");
        }
    }
}
